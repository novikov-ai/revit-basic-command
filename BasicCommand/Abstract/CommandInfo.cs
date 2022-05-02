namespace BasicCommand.Abstract
{
    /// <summary>
    /// Information about RibbonItem
    /// </summary>
    public interface CommandInfo
    {
        /// <summary>
        /// Displayed name
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Displayed description
        /// </summary>
        public string Description { get; }
       
        /// <summary>
        /// Displayed large picture 32x32 located at Resources/picture.png
        /// </summary>
        public string Picture { get; }
        
        /// <summary>
        /// Displayed version when the mouse hovers over the command for a long amount of time
        /// </summary>
        public string Version { get; }
    }
}
