namespace EditPdf
{
    /// <summary>
    /// Simple command-line argument parser for EditPdf operations.
    /// Provides structured access to parsed CLI arguments with both positional and named syntax support.
    /// </summary>
    public class CliArgumentParser
    {
        public string? InputFile { get; set; }
        public string? Action { get; set; }
        public string? OutputFile { get; set; }
        public bool SilentMode { get; set; }

        // Common options for multiple operations
        public int? StartPage { get; set; }
        public int? EndPage { get; set; }
        public int? Position { get; set; }
        public int? Count { get; set; }
        public int? Degrees { get; set; }
        public string? TextValue { get; set; }
        public string? Level { get; set; }
        public string? Size { get; set; }

        // Metadata options
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Subject { get; set; }
        public string? Keywords { get; set; }
        public string? Creator { get; set; }

        // Other options
        public string? WatermarkText { get; set; }
        public string? WatermarkPosition { get; set; }
        public int? WatermarkOpacity { get; set; }
        public string[] FilesToMerge { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Parses command-line arguments into a structured format.
        /// Supports both positional and named argument syntax.
        /// </summary>
        public static CliArgumentParser Parse(string[] args)
        {
            var parser = new CliArgumentParser();

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                switch (arg.ToLowerInvariant())
                {
                    case "--input":
                        parser.InputFile = i + 1 < args.Length ? args[++i] : null;
                        break;
                    case "--output":
                        parser.OutputFile = i + 1 < args.Length ? args[++i] : null;
                        break;
                    case "--silent":
                        parser.SilentMode = true;
                        break;
                    case "--start":
                    case "-s":
                        if (i + 1 < args.Length && int.TryParse(args[++i], out int start))
                            parser.StartPage = start;
                        break;
                    case "--end":
                    case "-e":
                        if (i + 1 < args.Length && int.TryParse(args[++i], out int end))
                            parser.EndPage = end;
                        break;
                    case "--position":
                    case "-pos":
                        if (i + 1 < args.Length)
                        {
                            string posArg = args[++i];
                            if (int.TryParse(posArg, out int pos))
                                parser.Position = pos;
                            else
                                parser.WatermarkPosition = posArg;
                        }
                        break;
                    case "--count":
                    case "-c":
                        if (i + 1 < args.Length && int.TryParse(args[++i], out int count))
                            parser.Count = count;
                        break;
                    case "--degrees":
                    case "-d":
                        if (i + 1 < args.Length && int.TryParse(args[++i], out int deg))
                            parser.Degrees = deg;
                        break;
                    case "--level":
                    case "-l":
                        parser.Level = i + 1 < args.Length ? args[++i] : null;
                        break;
                    case "--size":
                        parser.Size = i + 1 < args.Length ? args[++i] : null;
                        break;
                    case "--title":
                        parser.Title = i + 1 < args.Length ? args[++i] : null;
                        break;
                    case "--author":
                        parser.Author = i + 1 < args.Length ? args[++i] : null;
                        break;
                    case "--subject":
                        parser.Subject = i + 1 < args.Length ? args[++i] : null;
                        break;
                    case "--keywords":
                        parser.Keywords = i + 1 < args.Length ? args[++i] : null;
                        break;
                    case "--creator":
                        parser.Creator = i + 1 < args.Length ? args[++i] : null;
                        break;
                    case "--text":
                    case "-t":
                        parser.TextValue = i + 1 < args.Length ? args[++i] : null;
                        parser.WatermarkText = parser.TextValue;
                        break;
                    case "--opacity":
                        if (i + 1 < args.Length && int.TryParse(args[++i], out int opacity))
                            parser.WatermarkOpacity = opacity;
                        break;
                    case "--type":
                        parser.Level = i + 1 < args.Length ? args[++i] : null;
                        break;
                    case "--order":
                    case "--mode":
                    case "-m":
                    case "--files":
                    case "--source":
                    case "--page":
                    case "-p":
                    case "--start-number":
                    case "--font-size":
                        // These are handled by specific operations, skip for now
                        if (i + 1 < args.Length) i++;
                        break;
                    default:
                        // Handle positional arguments
                        if (!arg.StartsWith("--") && !arg.StartsWith("-"))
                        {
                            if (parser.InputFile == null && System.IO.File.Exists(arg))
                                parser.InputFile = arg;
                            else if (parser.Action == null)
                                parser.Action = arg;
                            else if (parser.OutputFile == null)
                                parser.OutputFile = arg;
                        }
                        break;
                }
            }

            return parser;
        }

        /// <summary>
        /// Validates that required arguments are present.
        /// </summary>
        public bool IsValid(out string? errorMessage)
        {
            if (string.IsNullOrEmpty(InputFile))
            {
                errorMessage = "Error: Input PDF file is required (--input <file> or positional)";
                return false;
            }

            if (string.IsNullOrEmpty(Action))
            {
                errorMessage = "Error: Action is required (see help for available actions)";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
