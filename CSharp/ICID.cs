﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Opentieba
{
    public class ICID : List<BDUSS>
    {
        public ICID(FileStream fs)
        {
            StreamReader sr = new StreamReader(fs);
            String p=sr.ReadToEnd();
            explainICID(p);
        }
        public ICID(String icidStr)
        {
            explainICID(icidStr);
        }
        protected void explainICID(String icidStr)
        {
            String[] lines = icidStr.Split("\n".ToCharArray());
            try
            {
                foreach (String line in lines)
                {
                    String linep = line;
                    if (line.EndsWith("\r"))
                    {
                        linep = line.Substring(0, line.Length - 1);
                    }
                    if (line.Length < 1)
                    {
                        continue;
                    }
                    if (line.StartsWith(";"))
                    {
                        continue;
                    }
                    //           111
                    // 0123456789012
                    // (a,)[BDUSS=b]
                    //           1111111
                    // 01234567890123456
                    // (abc,)[BDUSS=bbc]
                    if (line.StartsWith("("))
                    {
                        int usernameEnd = line.IndexOf(',', 1) - 1;
                        String username=line.Substring(1, usernameEnd);
                        int bdussStart = line.IndexOf("[BDUSS=", usernameEnd + 1) + 7;
                        String bduss = linep.Substring(bdussStart);
                        bduss = bduss.Substring(0, bduss.Length - 1);
                        this.Add(new BDUSS(username, bduss));
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
            }
            catch (Exception e)
            {
                throw new FormatException("ICID 格式不正确。参见文档“The ICID standard”");
            }
        }
    }
}