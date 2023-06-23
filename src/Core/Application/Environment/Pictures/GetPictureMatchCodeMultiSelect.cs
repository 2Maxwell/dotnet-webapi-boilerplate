using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Environment.Pictures;
public class GetPictureMatchCodeMultiSelectRequest : IRequest<List<string>>
{
    public int MandantId { get; set; }
    public GetPictureMatchCodeMultiSelectRequest(int mandantId)
    {
        MandantId = mandantId;
    }
}

public class PictureMatchCodeByMandantIdSpec : Specification<Picture, string>
{
    public PictureMatchCodeByMandantIdSpec(int mandantId)
    {
        Query.Select(c => c.MatchCode).Where(c => c.MandantId == mandantId);
    }
}

public class GetPictureMatchCodeMultiSelectRequestHandler : IRequestHandler<GetPictureMatchCodeMultiSelectRequest, List<string>>
{
    private readonly IRepository<Picture> _repository;
    private readonly IStringLocalizer<GetPictureMatchCodeMultiSelectRequestHandler> _localizer;

    public GetPictureMatchCodeMultiSelectRequestHandler(IRepository<Picture> repository, IStringLocalizer<GetPictureMatchCodeMultiSelectRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<List<string>> Handle(GetPictureMatchCodeMultiSelectRequest request, CancellationToken cancellationToken)
    {
        List<string> matchCodes = await _repository.ListAsync((ISpecification<Picture, string>)new PictureMatchCodeByMandantIdSpec(request.MandantId), cancellationToken);
        List<string> distinctMatchCodes = new List<string>();

        foreach (var item in matchCodes)
        {
            if (item.Contains(','))
            {
                string[] strings = item.Split(',');
                for (int i = 0; i < strings.Length; i++)
                {
                    if (!distinctMatchCodes.Contains(strings[i].Trim()))
                    {
                        distinctMatchCodes.Add(strings[i].Trim());
                    }
                }
            }
            else
            {
                if (!distinctMatchCodes.Contains(item.Trim()))
                {
                    string treffer = (string)item.Trim();
                    distinctMatchCodes.Add(treffer.Trim());
                }
            }
        }

        string eintrag1 = "Property";
        if(!distinctMatchCodes.Contains(eintrag1)) { distinctMatchCodes.Add(eintrag1); }
        string eintrag2 = "Restaurant";
        if (!distinctMatchCodes.Contains(eintrag2)) { distinctMatchCodes.Add(eintrag2); }
        string eintrag3 = "Meeting";
        if (!distinctMatchCodes.Contains(eintrag3)) { distinctMatchCodes.Add(eintrag3); }
        string eintrag4 = "Breakfast";
        if (!distinctMatchCodes.Contains(eintrag4)) { distinctMatchCodes.Add(eintrag4); }
        string eintrag5 = "Reception";
        if (!distinctMatchCodes.Contains(eintrag5)) { distinctMatchCodes.Add(eintrag5); }
        string eintrag6 = "Facility";
        if (!distinctMatchCodes.Contains(eintrag6)) { distinctMatchCodes.Add(eintrag6); }
        string eintrag7 = "Menu";
        if (!distinctMatchCodes.Contains(eintrag7)) { distinctMatchCodes.Add(eintrag7); }
        string eintrag8 = "Package";
        if (!distinctMatchCodes.Contains(eintrag8)) { distinctMatchCodes.Add(eintrag8); }

        return distinctMatchCodes;
    }

}