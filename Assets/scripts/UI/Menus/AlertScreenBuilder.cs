using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions.UI.Menus
{
    public class AlertScreenBuilder : MonoBehaviour
    {
        private string headerText = "header text";
        private string bodyText = "body text";
        private string buttonText = "OK";
        private AlertScreen alertScreen;
        public AlertScreenBuilder WithHeader(string header)
        {
            headerText = header;
            return this;
        }

        public AlertScreenBuilder WithBody(string body)
        {
            bodyText = body;
            return this;
        }

        public AlertScreenBuilder WithButtonText(string button)
        {
            buttonText = button;
            return this;
        }

        public void Build()
        {
            alertScreen.Header.SetText(headerText);
            alertScreen.Body.SetText(bodyText);
            alertScreen.Button.SetText(buttonText);
            alertScreen.Open();
        }

        public AlertScreenBuilder(AlertScreen screen)
        {
            alertScreen = screen;
        }
    }

}