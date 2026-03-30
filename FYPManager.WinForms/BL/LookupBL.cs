using FYPManager.WinForms.DAL;
using FYPManager.WinForms.Models;

namespace FYPManager.WinForms.BL;

public sealed class LookupBL
{
    private readonly LookupDAL _lookupDal;

    public LookupBL(LookupDAL lookupDal)
    {
        _lookupDal = lookupDal;
    }

    public Task<IReadOnlyList<Lookup>> GetByCategoryAsync(string category) => _lookupDal.GetByCategoryAsync(category);
    public Task<IReadOnlyList<Lookup>> GetGendersAsync() => _lookupDal.GetByCategoryAsync("GENDER");
    public Task<IReadOnlyList<Lookup>> GetDesignationsAsync() => _lookupDal.GetByCategoryAsync("DESIGNATION");
    public Task<IReadOnlyList<Lookup>> GetStatusesAsync() => _lookupDal.GetByCategoryAsync("STATUS");
    public Task<IReadOnlyList<Lookup>> GetAdvisorRolesAsync() => _lookupDal.GetByCategoryAsync("ADVISOR_ROLE");
}
