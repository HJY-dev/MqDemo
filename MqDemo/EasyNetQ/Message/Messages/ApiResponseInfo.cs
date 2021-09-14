using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    /// <summary>
    /// API接口统一返回格式
    /// </summary>
    public class ApiResponseInfo
    {
        public enum ResponseCode
        {
            /// <summary>
            /// 成功
            /// </summary>
            [Description("成功")]
            SUCCESS = 0,

            /// <summary>
            /// 错误
            /// </summary>
            [Description("错误")]
            ERROR = 1,

            /// <summary>
            /// 未登录
            /// </summary>
            [Description("未登录")]
            NOLOGIN = 2,

            /// <summary>
            /// 操作提示信息
            /// </summary>
            TOAST = 3,

            /// <summary>
            /// 查询不到数据
            /// </summary>
            [Description("查询不到数据")]
            NODATA = 4,

            /// <summary>
            /// 参数错误
            /// </summary>
            [Description("参数错误")]
            PARAM_ERROR = -100,

            /// <summary>
            /// 服务器错误
            /// </summary>
            [Description("服务器错误")]
            SERVER_ERROR = 500


        }
        /// <summary>
        /// 0:成功,1:错误,2:操作提示信息，-100：参数错误
        /// </summary>
        public ResponseCode Code { get; set; }

        /// <summary>
        /// 接口返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 接口返回数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// API返回数据统一格式
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <param name="code">返回数据状态</param>
        /// <param name="message">信息</param>
        /// <returns></returns>
        public static ApiResponseInfo Response(object data, ResponseCode code = ResponseCode.SUCCESS, string message = null)
        {
            ApiResponseInfo responseInfo = new ApiResponseInfo();
            responseInfo.Code = code;
            responseInfo.Message = message;
            responseInfo.Data = data;
            return responseInfo;
        }

    }
}
