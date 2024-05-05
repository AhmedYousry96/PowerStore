﻿using PowerStore.Api.DTOs.Catalog;
using MediatR;

namespace PowerStore.Api.Commands.Models.Catalog
{
    public class DeleteProductPictureCommand : IRequest<bool>
    {
        public ProductDto Product { get; set; }
        public string PictureId { get; set; }
    }
}
