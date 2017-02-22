using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FaceSysClient.ClassPool
{
    public class validationRule
    {
        WriteLog _WriteLog = new WriteLog();
        /// <summary>
        /// 正整数验证
        /// </summary>
        public string intValidationRule(string strValidationRule)
        {
            try
			{
				#region
				string message = "";
				// 增加长度是否为1的判断，如果长度可以为1，那么字符可以为0.
				if (strValidationRule.Length == 1)
				{
					int n = strValidationRule[0];
					if (n < 48 || n >= 58)
					{
						message = "格式错误！";
					}
					return message;
				}
				else
				{
					for (int i = 0; i < strValidationRule.Length; i++)
					{
						int n = strValidationRule[i];
						if (i == 0)
						{
							if (n <= 48 || n >= 58)
							{
								message = "数字第一位输入不能为0或者格式错误！";
								break;
							}
						}
						else
						{
							if (n < 48 || n >= 58)
							{
								message = "输入错误，只能输入数字！";
								break;
							}
						}
					}
					return message;
				}
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("intValidationRule", ex);
                return "";
                //System.Windows.MessageBox.Show(ex.Message); 
            }
        }
        public string intValidationRule1(string strValidationRule)
        {
            try
            {
                #region
                string message = "";
                for (int i = 0; i < strValidationRule.Length; i++)
                {
                    int n = strValidationRule[i];
                    if (n < 48 || n >= 58)
                    {
                        message = "输入错误，只能输入数字！";
                        break;
                    }
                    if (strValidationRule.Length != 1)//输入的数据长度不为1，首位数字不能为0
                    {
                        if (i == 0)
                        {
                            if (n <= 48 || n >= 58)
                            {
                                message = "数字第一位输入不能为0或者格式错误！";
                                break;
                            }
                        }
                    }
                }
                return message;
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("intValidationRule1", ex);
                return "";
                //System.Windows.MessageBox.Show(ex.Message); 
            }
        }
        public string ipValidationRule(string IpValidationRule)
        {
            try
            {
                #region
                string IPAddress = IpValidationRule as string;
                string str = "";
                if (!string.IsNullOrWhiteSpace(IPAddress))
                {
                    string IPAddressFormartRegex = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";

                    // 检查输入的字符串是否符合IP地址格式
                    if (!Regex.IsMatch(IPAddress, IPAddressFormartRegex))
                    {
                        str = "IP地址格式不正确";
                    }
                }

                return str;
                #endregion
            }
            catch (Exception ex)
            {
                _WriteLog.WriteToLog("ipValidationRule", ex);
                return "";
                //System.Windows.MessageBox.Show(ex.Message); 
            }
        }
    }
}
