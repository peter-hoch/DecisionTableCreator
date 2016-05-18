using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.Utils
{
    public class PrepareForClipboard
    {
        public int StartHtml { get; private set; }
        public int EndHtml { get; private set; }
        public int StartFragment { get; private set; }
        public int EndFragment { get; private set; }

        public string Prepare(string html)
        {
            Encoding encoding = Encoding.UTF8;

            StringBuilder htmlPrefixText = new StringBuilder();
            htmlPrefixText.AppendFormat("<html>").AppendLine();
            htmlPrefixText.AppendFormat("<head>").AppendLine();
            htmlPrefixText.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\">", Encoding.UTF8.WebName).AppendLine();
            htmlPrefixText.AppendFormat("<title>HTML clipboard</title>").AppendLine();
            htmlPrefixText.AppendFormat("</head>").AppendLine();
            htmlPrefixText.AppendFormat("<body>").AppendLine();

            StringBuilder beforeHtml = new StringBuilder();
            beforeHtml.AppendLine("Version:0.9");
            beforeHtml.AppendLine("StartHTML:{0:000000}");
            beforeHtml.AppendLine("EndHTML:{1:000000}");
            beforeHtml.AppendLine("StartFragment:{2:000000}");
            beforeHtml.AppendLine("EndFragment:{3:000000}");

            string endHtmlText = "<!--EndFragment-->" + Environment.NewLine + "</body>" + Environment.NewLine + "</html>" + Environment.NewLine;
            string fragmentText = "<!--StartFragment-->" + html;

            StartHtml = encoding.GetByteCount(String.Format(beforeHtml.ToString(), 0, 0, 0, 0));
            int htmlPrefix = encoding.GetByteCount(htmlPrefixText.ToString());
            StartFragment = StartHtml + htmlPrefix;
            int fragment = encoding.GetByteCount(fragmentText);
            EndFragment = StartFragment + fragment;
            EndHtml = EndFragment + encoding.GetByteCount(endHtmlText);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(beforeHtml.ToString(), StartHtml, EndHtml, StartFragment, EndFragment);
            sb.Append(htmlPrefixText);
            sb.Append(fragmentText);
            sb.Append(endHtmlText);
            return sb.ToString();
        }
    }
}
