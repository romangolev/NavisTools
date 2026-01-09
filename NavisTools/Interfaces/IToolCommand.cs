namespace NavisTools.Interfaces
{
    /// <summary>
    /// Represents a tool command that can be executed.
    /// Implements the Command pattern for tool execution.
    /// </summary>
    public interface IToolCommand
    {
        /// <summary>
        /// Gets the unique identifier for this command.
        /// </summary>
        string CommandId { get; }

        /// <summary>
        /// Gets whether the command can currently be executed.
        /// </summary>
        bool CanExecute { get; }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns>0 for success, non-zero for failure.</returns>
        int Execute();
    }
}
