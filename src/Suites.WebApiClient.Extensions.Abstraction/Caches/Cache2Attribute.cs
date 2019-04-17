//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using WebApiClient.Attributes;
//using WebApiClient.Contexts;

//namespace WebApiClient.Extensions.Caches
//{
//    public class Cache2Attribute : ApiActionAttribute
//    {
//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="secAge">缓存有效时间，单位：秒</param>
//        public Cache2Attribute(double secAge)
//        {
//            NotTrueException.Assert(secAge > 0, nameof(secAge));
//            SecAge = TimeSpan.FromSeconds(secAge);
//        }

//        public TimeSpan SecAge { get; }

//        public override Task BeforeRequestAsync(ApiActionContext context)
//            => TaskEx.CompletedTask;
//    }
//}