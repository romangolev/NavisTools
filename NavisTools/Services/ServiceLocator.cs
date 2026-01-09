using NavisTools.Interfaces;
using System;
using System.Collections.Generic;

namespace NavisTools.Services
{
    /// <summary>
    /// Simple service locator for dependency injection.
    /// Provides a central point for service registration and resolution.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private static readonly Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>();
        private static bool _isInitialized;
        private static readonly object _lock = new object();

        /// <summary>
        /// Initializes the service locator with default services.
        /// </summary>
        public static void Initialize()
        {
            lock (_lock)
            {
                if (_isInitialized)
                    return;

                // Register default implementations
                var documentProvider = new NavisworksDocumentProvider();
                Register<IDocumentProvider>(documentProvider);

                var selectionService = new NavisworksSelectionService(documentProvider);
                Register<ISelectionService>(selectionService);

                var notificationService = new NotificationService();
                Register<INotificationService>(notificationService);

                var configurationService = new ConfigurationService();
                Register<IConfigurationService>(configurationService);

                _isInitialized = true;
            }
        }

        /// <summary>
        /// Registers a service instance.
        /// </summary>
        public static void Register<TService>(TService instance) where TService : class
        {
            lock (_lock)
            {
                _services[typeof(TService)] = instance;
            }
        }

        /// <summary>
        /// Registers a factory function for creating service instances.
        /// </summary>
        public static void RegisterFactory<TService>(Func<TService> factory) where TService : class
        {
            lock (_lock)
            {
                _factories[typeof(TService)] = () => factory();
            }
        }

        /// <summary>
        /// Resolves a service instance.
        /// </summary>
        public static TService Resolve<TService>() where TService : class
        {
            // Ensure initialization
            if (!_isInitialized)
            {
                Initialize();
            }

            lock (_lock)
            {
                // Try to get existing instance
                if (_services.TryGetValue(typeof(TService), out var service))
                {
                    return (TService)service;
                }

                // Try to create from factory
                if (_factories.TryGetValue(typeof(TService), out var factory))
                {
                    var instance = (TService)factory();
                    _services[typeof(TService)] = instance;
                    return instance;
                }

                throw new InvalidOperationException($"Service of type {typeof(TService).Name} is not registered.");
            }
        }

        /// <summary>
        /// Tries to resolve a service instance.
        /// </summary>
        public static bool TryResolve<TService>(out TService service) where TService : class
        {
            try
            {
                service = Resolve<TService>();
                return true;
            }
            catch
            {
                service = null;
                return false;
            }
        }

        /// <summary>
        /// Resets the service locator (primarily for testing).
        /// </summary>
        public static void Reset()
        {
            lock (_lock)
            {
                _services.Clear();
                _factories.Clear();
                _isInitialized = false;
            }
        }
    }
}
