﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFeatures.UserControls
{
    public partial class AdBanner : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string AdBannerImage
        {
            get { return "http://www.economypaper.com/images/banner_placeholder.jpg"; }
        }
    }
}