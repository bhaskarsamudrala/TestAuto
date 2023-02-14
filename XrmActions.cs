// @author    : Mahesh Gunipati
// @filename  : XrmActions.cs
// @date      : 09/20/2022

using System;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Company.Platform.automation.App.web.Common
{
    public class XrmActions
    {
        private readonly XrmApp _xrmApp;

        public XrmActions(XrmApp xrmApp)
        {
            _xrmApp = xrmApp;
        }

        //SubArea

        public void OpenSubArea(string subAreaName)
        {
            _xrmApp.Navigation.OpenSubArea(subAreaName);
        }

        // Selections

        public void SelectTab(string tabName)
        {
            _xrmApp.Entity.SelectTab(tabName);
        }


        //Set and Get Methods

        public void SetValue(string identifierName, string value)
        {
            var _option = new OptionSet
            {
                Name = identifierName,
                Value = value
            };
            _xrmApp.Entity.SetValue(_option);
        }

        public string GetEntityValue(string entityIdentifier)
        {
            return _xrmApp.Entity.GetValue(entityIdentifier);
        }

        public Boolean GetBooleanValue(string identifierName)
        {
            return _xrmApp.Entity.GetValue(new BooleanItem { Name = identifierName });
        }

        public void SetBooleanValue(string identifierName, Boolean bValue)
        {
            _xrmApp.Entity.SetValue(new BooleanItem { Name = identifierName, Value = bValue });
        }


        //CommandBar

        public void ClickCommand(string commandName)
        {
            _xrmApp.CommandBar.ClickCommand(commandName);
        }

        public void ClickCommandAndSelectValue(string commandName, string value)
        {
            _xrmApp.CommandBar.ClickCommand(commandName, value);
        }

        //Grid 
        public void SwitchToGridView(string applicationType)
        {
            _xrmApp.Grid.SwitchView(applicationType);
        }

        public void OpenRecordByNumber(int number)
        {
            _xrmApp.Grid.OpenRecord(number);
        }

        // SubGrid
        public void clickSubGridCommand(string subGridName, string commandName)
        {
            _xrmApp.Entity.SubGrid.ClickCommand(subGridName, commandName);
        }

    }
}