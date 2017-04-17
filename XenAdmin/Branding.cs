﻿/* Copyright (c) Citrix Systems, Inc. 
 * All rights reserved. 
 * 
 * Redistribution and use in source and binary forms, 
 * with or without modification, are permitted provided 
 * that the following conditions are met: 
 * 
 * *   Redistributions of source code must retain the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer. 
 * *   Redistributions in binary form must reproduce the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer in the documentation and/or other 
 *     materials provided with the distribution. 
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
 * SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Text;

// Values taken from branding.hg

namespace XenAdmin
{
    static public class Branding
    {
        public const string PRODUCT_VERSION_TEXT = "6.0";
        public const string XENCENTER_VERSION = "6.0.1";
        public const string COMPANY_NAME_LEGAL = "Halsign Corporation";
        public const string BRAND_CONSOLE = "vGate";
        public const string PRODUCT_BRAND = "vGate";
        public const string COMPANY_NAME_SHORT = "Halsign";
        public const string SEARCH = "vGatesearch";
        public const string UPDATE = "[xsupdate]";
        public const string UPDATEISO = "[iso]";
        public const string BACKUP = "[xbk]";
        public const string CHECK_FOR_UPDATES_URL = "[BRANDING_XENSERVER_UPDATE_URL]";
        public const string BUILD_NUMBER = "20160601";
        public const string COPYRIGHT_YEARS = "2016";

        public static string Search
        {
            get
            {
                var s = SEARCH;
                return s != "[" + "xensearch]" ? s : InvisibleMessages.XEN_SEARCH;
            }
        }
        
        public static string Update
        {
            get
            {
                var s = UPDATE;
                return s != "[" + "xsupdate]" ? s : InvisibleMessages.XEN_UPDATE;
            }
        }

        public static string UpdateIso
        {
            get
            {
                var s = UPDATEISO;
                return s != "[" + "iso]" ? s : InvisibleMessages.ISO_UPDATE;
            }
        }

        public static string CheckForUpdatesUrl
        {
            get
            {
                var s = CHECK_FOR_UPDATES_URL;
                return s != "[" + "BRANDING_XENSERVER_UPDATE_URL]" ? s : InvisibleMessages.XENSERVER_UPDATE_URL;
            }
        }
    }
}
