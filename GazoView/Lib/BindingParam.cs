namespace GazoView.Lib
{
    public class BindingParam
    {
        /// <summary>
        /// Application settings.
        /// </summary>
        public Setting Setting { get; set; }

        /// <summary>
        /// Image viewer instance.
        /// </summary>
        public Images Images { get; set; }

        /// <summary>
        /// For trimming mode instance.
        /// </summary>
        public Trimming Trimming { get; set; }

        /// <summary>
        /// For scaling mode instance.
        /// </summary>
        public ScaleRate ScaleRate { get; set; }

        /// <summary>
        /// For RenameBox window.
        /// </summary>
        public RenameBox RenameBox { get; set; }

        /// <summary>
        /// For DeleteMessage window.
        /// </summary>
        public DeleteMessage DeleteMessage { get; set; }
    }
}
