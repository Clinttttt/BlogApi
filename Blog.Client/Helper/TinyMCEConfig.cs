namespace Blog.Client.Helper
{
    public static class TinyMCEConfig
    {
        public static Dictionary<string, object> GetConfig()
        {
            return new Dictionary<string, object>
            {
                { "menubar", false },
                { "branding", false },
                { "resize", false },
                { "promotion", false },
                { "statusbar", false },
                { "highlight_on_focus", false },
                { "placeholder", "Start writing your content here..." },
                { "plugins", "lists link image code codesample table paste autoresize visualblocks fullscreen" },
                { "toolbar", "undo redo | styles | bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image table | codesample fullscreen | removeformat code" },

                { "codesample_languages", new[] {
                    new { text = "HTML/XML", value = "markup" },
                    new { text = "JavaScript", value = "javascript" },
                    new { text = "TypeScript", value = "typescript" },
                    new { text = "CSS", value = "css" },
                    new { text = "C#", value = "csharp" },
                    new { text = "Python", value = "python" },
                    new { text = "Java", value = "java" },
                    new { text = "C++", value = "cpp" },
                    new { text = "PHP", value = "php" },
                    new { text = "Ruby", value = "ruby" },
                    new { text = "Go", value = "go" },
                    new { text = "Rust", value = "rust" },
                    new { text = "SQL", value = "sql" },
                    new { text = "JSON", value = "json" },
                    new { text = "Bash/Shell", value = "bash" }
                }},

                { "codesample_global_prismjs", true },
                { "paste_as_text", false },
                { "paste_merge_formats", true },
                { "paste_retain_style_properties", "all" },
                { "paste_remove_styles_if_webkit", false },
                { "paste_word_valid_elements", "b,strong,i,em,u,s,h1,h2,h3,h4,h5,h6,p,ul,ol,li,a[href],span[style],table,tr,td,th,thead,tbody,div[style],br,code,pre" },
                { "paste_webkit_styles", "all" },
                { "paste_preprocess", true },
                { "paste_enable_default_filters", false },
                { "paste_data_images", true },

                { "content_style", ContentStyle }
            };
        }

        private const string ContentStyle = @"
            @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap');

            body {
                font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
                font-size: 16px;
                line-height: 1.8;
                color: #1f2937;
                padding: 20px;
                background: #ffffff;
            }

            h1, h2, h3, h4, h5, h6 {
                font-weight: 700;
                line-height: 1.3;
                margin-top: 1.5em;
                margin-bottom: 0.5em;
                color: #111827;
            }

            h1 { font-size: 2.25em; }
            h2 { font-size: 1.875em; }
            h3 { font-size: 1.5em; }
            h4 { font-size: 1.25em; }

            p { margin-bottom: 1.25em; }

            strong, b {
                font-weight: 600;
                color: #111827;
            }

            em, i { font-style: italic; }
            u { text-decoration: underline; }

            a {
                color: #6366f1;
                text-decoration: underline;
            }

            code {
                background: #f3f4f6;
                color: #dc2626;
                padding: 3px 8px;
                border-radius: 6px;
                font-family: 'SF Mono', 'Monaco', 'Menlo', 'Consolas', monospace;
                font-size: 0.9em;
                font-weight: 500;
                border: 1px solid #e5e7eb;
            }

            pre {
                padding: 20px !important;
                margin: 1.5em 0 !important;
                overflow-x: auto !important;
                font-family: 'SF Mono', 'Monaco', 'Menlo', 'Consolas', monospace !important;
                font-size: 14px !important;
                line-height: 1.7 !important;
            }

            pre code {
                background: none !important;
                padding: 0 !important;
                color: #e5e7eb !important;
                border: none !important;
                display: block !important;
            }

            ul, ol {
                padding-left: 1.5em;
                margin-bottom: 1.25em;
            }

            li { margin-bottom: 0.5em; }

            table {
                width: 100%;
                border-collapse: collapse;
                margin: 1.5em 0;
                border: 1px solid #e5e7eb;
                border-radius: 8px;
                overflow: hidden;
            }

            th {
                background: #f9fafb;
                padding: 12px;
                text-align: left;
                font-weight: 600;
                border-bottom: 2px solid #e5e7eb;
            }

            td {
                padding: 12px;
                border-bottom: 1px solid #e5e7eb;
            }

            blockquote {
                border-left: 4px solid #6366f1;
                padding-left: 1.5em;
                margin: 1.5em 0;
                color: #4b5563;
                font-style: italic;
                padding: 1em 1.5em;
                border-radius: 0 8px 8px 0;
            }

            img {
                max-width: 100%;
                height: auto;
                border-radius: 8px;
                margin: 1.5em 0;
            }
        ";
    }
}
