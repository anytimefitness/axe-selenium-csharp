﻿using System;
using System.Net;

namespace Globant.Selenium.Axe
{
    public class ExternalAxeScriptProvider : IAxeScriptProvider
    {
        private readonly Uri _scriptUri;
        private readonly IContentDownloader _contentDownloader;

        public ExternalAxeScriptProvider(Uri scriptUri)
        {
            using (WebClient webClient = new WebClient())
            {
                if (webClient == null)
                    throw new ArgumentNullException(nameof(webClient));

                if (scriptUri == null)
                    throw new ArgumentNullException(nameof(scriptUri));

                _scriptUri = scriptUri;
                _contentDownloader = new CachedContentDownloader(webClient);
            }
        }

        public string GetScript() => _contentDownloader.GetContent(_scriptUri);
    }
}
