namespace FSH.WebApi.Application.Accounting.Mandants;

public class MandantFullDto : IDto
{
    public MandantDto? MandantDto { get; set; }
    public MandantDetailDto? MandantDetailDto { get; set; }
    public MandantSettingDto? MandantSettingDto { get; set; }
    public MandantNumbersDto? MandantNumbersDto { get; set; }
}