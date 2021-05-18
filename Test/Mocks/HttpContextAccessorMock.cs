using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Mocks
{
    internal sealed class HttpContextAccessorMock : Mock<IHttpContextAccessor>
    {
        public HttpContextAccessorMock()
        {
            this.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
        }
    }

}
