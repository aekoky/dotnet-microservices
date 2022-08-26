namespace TemplateService.Business.DTO
{
    public class CreateTemplateDTO
    {
        public string Label { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public byte[] File { get; set; }
        public string OwnerId { get; set; }
    }
}
