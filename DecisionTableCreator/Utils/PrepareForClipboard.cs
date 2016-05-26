/*
 * [The "BSD license"]
 * Copyright (c) 2016 Peter Hoch
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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
