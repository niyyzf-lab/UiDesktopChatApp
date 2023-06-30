using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopChatApp.Tools
{
    internal class AbbreviationGenerator
    {
        public static string GenerateAbbreviation(string fullName)
        {
            // 检查名字是否为空
            if (string.IsNullOrEmpty(fullName))
            {
                return string.Empty;
            }

            // 按空格分割名字
            string[] nameParts = fullName.Split(' ');

            // 检查名字部分数量
            if (nameParts.Length == 1)
            {
                // 如果只有一个名字部分，返回姓氏作为缩写
                return GetChineseSurname(nameParts[0]);
            }
            else
            {
                // 否则，检查第一个名字部分的类型
                if (IsEnglishName(nameParts[0]))
                {
                    // 如果是英文名，返回所有名字部分的首字母大写
                    return GetEnglishAbbreviation(nameParts);
                }
                else
                {
                    // 如果是中文名，返回姓氏作为缩写
                    return GetChineseSurname(nameParts[0]);
                }
            }
        }

        private static bool IsEnglishName(string name)
        {
            // 检查名字中是否包含字母来判断是否是英文名
            foreach (char c in name)
            {
                if (char.IsLetter(c))
                {
                    return true;
                }
            }

            return false;
        }

        private static string GetChineseSurname(string name)
        {
            // 返回中文名的姓氏
            if (name.Length >= 1)
            {
                return name[..1];
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetEnglishAbbreviation(string[] nameParts)
        {
            // 返回所有英文名字部分的首字母大写
            string abbreviation = string.Empty;

            foreach (string namePart in nameParts)
            {
                if (!string.IsNullOrEmpty(namePart))
                {
                    abbreviation += namePart[..1].ToUpper();
                }
            }

            return abbreviation;
        }
    }
}
