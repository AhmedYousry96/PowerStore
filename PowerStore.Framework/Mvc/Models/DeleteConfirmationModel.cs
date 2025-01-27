﻿using PowerStore.Core.Models;

namespace PowerStore.Framework.Mvc.Models
{
    public class DeleteConfirmationModel : BaseEntityModel
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string WindowId { get; set; }
    }
}