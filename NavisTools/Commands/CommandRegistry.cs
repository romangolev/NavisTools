using NavisTools.Interfaces;
using NavisTools.Services;
using System.Collections.Generic;

namespace NavisTools.Commands
{
    /// <summary>
    /// Registry for all tool commands. Implements Open/Closed Principle
    /// by allowing new commands to be registered without modifying existing code.
    /// </summary>
    public class CommandRegistry
    {
        private readonly Dictionary<string, IToolCommand> _commands = new Dictionary<string, IToolCommand>();
        private static CommandRegistry _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets the singleton instance of the command registry.
        /// </summary>
        public static CommandRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CommandRegistry();
                            _instance.RegisterDefaultCommands();
                        }
                    }
                }
                return _instance;
            }
        }

        private CommandRegistry() { }

        /// <summary>
        /// Registers all default commands.
        /// </summary>
        private void RegisterDefaultCommands()
        {
            var documentProvider = ServiceLocator.Resolve<IDocumentProvider>();
            var selectionService = ServiceLocator.Resolve<ISelectionService>();
            var notificationService = ServiceLocator.Resolve<INotificationService>();
            var configurationService = ServiceLocator.Resolve<IConfigurationService>();

            // Register commands
            Register(new AddParentNameCommand(documentProvider, selectionService, notificationService, configurationService));
            Register(new TotalSumsCommand(documentProvider, selectionService, notificationService));
            Register(new OpenSettingsCommand(documentProvider, selectionService, notificationService, configurationService));
            Register(new ResetSettingsCommand(documentProvider, selectionService, notificationService, configurationService));
            Register(new SelectionInfoCommand(documentProvider, selectionService, notificationService));
            Register(new AboutCommand(documentProvider, selectionService, notificationService));
        }

        /// <summary>
        /// Registers a command with the registry.
        /// </summary>
        public void Register(IToolCommand command)
        {
            _commands[command.CommandId] = command;
        }

        /// <summary>
        /// Gets a command by its ID.
        /// </summary>
        public IToolCommand GetCommand(string commandId)
        {
            _commands.TryGetValue(commandId, out var command);
            return command;
        }

        /// <summary>
        /// Executes a command by its ID.
        /// </summary>
        public int ExecuteCommand(string commandId)
        {
            var command = GetCommand(commandId);
            if (command != null && command.CanExecute)
            {
                return command.Execute();
            }
            return 0;
        }

        /// <summary>
        /// Checks if a command can be executed.
        /// </summary>
        public bool CanExecuteCommand(string commandId)
        {
            var command = GetCommand(commandId);
            return command?.CanExecute ?? false;
        }

        /// <summary>
        /// Resets the registry (for testing).
        /// </summary>
        public static void Reset()
        {
            lock (_lock)
            {
                _instance = null;
            }
        }
    }
}
