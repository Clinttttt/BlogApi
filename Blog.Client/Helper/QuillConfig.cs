namespace Blog.Client.Helper
{
    public static class QuillConfig
    {
        public static string GetToolbarConfig()
        {
            return @"[
                [{""header"": [1, 2, 3, 4, 5, 6, false]}],
                [{""size"": [""small"", false, ""large"", ""huge""]}],
                [""bold"", ""italic"", ""underline"", ""strike""],
                [{""color"": []}, {""background"": []}],
                [{""script"": ""sub""}, {""script"": ""super""}],
                [{""list"": ""ordered""}, {""list"": ""bullet""}, {""indent"": ""-1""}, {""indent"": ""+1""}],
                [{""align"": []}],
                [""blockquote"", ""code-block""],
                [""link"", ""image"", ""video""],
                [""clean""]
            ]";
        }

        public static string GetContentStyle()
        {
            return @"
                @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap');

                .ql-container {
                    font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
                    font-size: 16px;
                    line-height: 1.8;
                    color: #1f2937;

                }

                .ql-editor {
                    min-height: 680px;
                    padding: 20px;
                    background: #ffffff;
                }

                .ql-editor.ql-blank::before {
                    content: 'Start writing your content here...' !important;
                    color: #9ca3af !important;
                    font-size: 16px !important;
                    font-style: normal !important;
                }

                .ql-editor h1,
                .ql-editor h2,
                .ql-editor h3,
                .ql-editor h4,
                .ql-editor h5,
                .ql-editor h6 {
                    font-weight: 700;
                    line-height: 1.3;
                    margin-top: 1.5em;
                    margin-bottom: 0.5em;
                    color: #111827;
                }

                .ql-editor h1 { font-size: 2.25em; }
                .ql-editor h2 { font-size: 1.875em; }
                .ql-editor h3 { font-size: 1.5em; }
                .ql-editor h4 { font-size: 1.25em; }

                .ql-editor p { 
                    margin-bottom: 1.25em;
                    margin-top: 0;
                }

                .ql-editor strong,
                .ql-editor b {
                    font-weight: 600;
                    color: #111827;
                }

                .ql-editor em,
                .ql-editor i { 
                    font-style: italic; 
                }

                .ql-editor u { 
                    text-decoration: underline; 
                }
                
                .ql-editor s,
                .ql-editor strike {
                    text-decoration: line-through;
                }
                
                .ql-editor sup { 
                    vertical-align: super; 
                    font-size: 0.75em; 
                }

                .ql-editor sub { 
                    vertical-align: sub; 
                    font-size: 0.75em; 
                }

                .ql-editor a {
                    color: #6366f1;
                    text-decoration: underline;
                }
                
                .ql-editor a:hover {
                    color: #4f46e5;
                }

                .ql-editor code {
                    background: #f3f4f6;
                    color: #dc2626;
                    padding: 3px 8px;
                    border-radius: 6px;
                    font-family: 'SF Mono', 'Monaco', 'Menlo', 'Consolas', monospace;
                    font-size: 0.9em;
                    font-weight: 500;
                    border: 1px solid #e5e7eb;
                }

                .ql-editor pre {
                    background: #1e293b !important;
                    padding: 20px !important;
                    margin: 1.5em 0 !important;
                    overflow-x: auto !important;
                    border-radius: 8px !important;
                }

                .ql-editor pre.ql-syntax {
                    background: #1e293b !important;
                    color: #e5e7eb !important;
                    font-family: 'SF Mono', 'Monaco', 'Menlo', 'Consolas', monospace !important;
                    font-size: 14px !important;
                    line-height: 1.7 !important;
                }

                .ql-editor ul,
                .ql-editor ol {
                    padding-left: 1.5em;
                    margin-bottom: 1.25em;
                    margin-top: 0;
                }

                .ql-editor li { 
                    margin-bottom: 0.5em; 
                }

                .ql-editor blockquote {
                    border-left: 4px solid #6366f1;
                    padding-left: 1.5em;
                    margin: 1.5em 0;
                    color: #4b5563;
                    font-style: italic;
                    padding: 1em 1.5em;
                    background: #f9fafb;
                    border-radius: 0 8px 8px 0;
                }

                .ql-editor img {
                    max-width: 100%;
                    height: auto;
                    border-radius: 8px;
                    margin: 1.5em 0;
                }

                /* Color picker */
                .ql-snow .ql-picker.ql-color .ql-picker-label,
                .ql-snow .ql-picker.ql-background .ql-picker-label {
                    width: 28px;
                }

                /* Toolbar styling */
                .ql-snow.ql-toolbar {
                    border: 1px solid #e5e7eb;
                    border-radius: 8px 8px 0 0;
                    background: #ffffff;
                    padding: 12px;

                }

                .ql-snow .ql-stroke {
                    stroke: #64748b;
                }

                .ql-snow .ql-fill {
                    fill: #64748b;
                }

                .ql-snow.ql-toolbar button:hover,
                .ql-snow .ql-toolbar button:hover,
                .ql-snow.ql-toolbar button.ql-active,
                .ql-snow .ql-toolbar button.ql-active {
                    color: #6366f1;
                }

                .ql-snow.ql-toolbar button:hover .ql-stroke,
                .ql-snow .ql-toolbar button:hover .ql-stroke,
                .ql-snow.ql-toolbar button.ql-active .ql-stroke,
                .ql-snow .ql-toolbar button.ql-active .ql-stroke {
                    stroke: #6366f1;
                }

                .ql-snow.ql-toolbar button:hover .ql-fill,
                .ql-snow .ql-toolbar button:hover .ql-fill,
                .ql-snow.ql-toolbar button.ql-active .ql-fill,
                .ql-snow .ql-toolbar button.ql-active .ql-fill {
                    fill: #6366f1;
                }

                /* Container styling */
                .ql-container.ql-snow {
                    border: 1px solid #e5e7eb;
                    border-top: none;
                    border-radius: 0 0 8px 8px;
                }
            ";
        }
    }
}