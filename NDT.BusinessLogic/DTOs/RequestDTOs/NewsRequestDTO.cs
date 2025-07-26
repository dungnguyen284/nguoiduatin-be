using System;
using System.Collections.Generic;
using NDT.BusinessModels.Entities;

namespace NDT.BusinessLogic.DTOs.RequestDTOs
{
    public class NewsRequestDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
        public string Source { get; set; }
        public NewsStatus Status { get; set; }
        public Guid? AuthorId { get; set; }
        public int CategoryId { get; set; }
        public List<int> TagIds { get; set; }
    }
}