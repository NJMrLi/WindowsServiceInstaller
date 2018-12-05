using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsServiceInstaller.InstallHelper
{
    public class ArgsParser
    {
        private readonly string[] _args;

        public ArgsParser(string[] args)
        {
            _args = args;
        }

        public string GetParamValueOrThrow(string paramName)
        {
            var paramValue = GetParamValueOrNull(paramName);
            if (paramValue == null)
            {
                throw new ArgumentException($"参数{paramName}的值不能为空", paramName);
            }
            return paramValue;
        }

        public string GetParamValueOrNull(string paramName)
        {
            if (_args == null)
            {
                return null;
            }

            var paramNameWithSlash = paramName.StartsWith("/") ? paramName : "/" + paramName;

            var paramList = _args.Where(x => x.StartsWith(paramNameWithSlash, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (paramList.Count == 0)
            {
                return null;
            }

            string result = GetParamValueFromMatchingList(paramNameWithSlash, paramList);
            return result;
        }

        private string GetParamValueFromMatchingList(string paramNameWithSlash, List<string> paramList)
        {
            // 参数出现次数
            int paramOccurrence = 0;
            string result = null;

            foreach (var paramListItem in paramList)
            {
                var arr = paramListItem.Split(new char[] { ':', '=' }, 2);

                // 通过字符串长度判断参数名称是否完全匹配, 有可能是子字符串
                if (arr[0].Length != paramNameWithSlash.Length)
                {
                    continue;
                }

                paramOccurrence++;

                if (arr.Length == 1)
                {
                    result = string.Empty;
                }
                else
                {
                    result = arr[1];
                }
            }

            if (paramOccurrence > 1)
            {
                throw new ArgumentException("参数个数大于1", paramNameWithSlash);
            }

            return result;
        }

        public bool HasParam(string paramName)
        {
            return GetParamValueOrNull(paramName) != null;
        }
    }
}
