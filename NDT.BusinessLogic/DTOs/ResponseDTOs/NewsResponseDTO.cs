using System;
using System.Collections.Generic;

namespace NDT.BusinessLogic.DTOs.ResponseDTOs
{
    public class NewsResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
        public string Source { get; set; }
        public bool IsActive { get; set; }
        public bool IsFrontPage { get; set; } = false;
        public int SlotNumber { get; set; } = 0; 
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<TagResponseDTO> Tags { get; set; }
        public int ViewCount { get; set; }
    }
} 