using DataAccess.Models;
using Domain.Data;
using System;
using System.Collections.Generic;

namespace Application.IServices
{
    public interface IServiceKey
    {
        List<Key> GetKeys();
        Key GetKey(int id);
        AssingnedKeyDto AssignKey(AssingnKeyDto assignKeyDto);
        AssingnedKeyDto Enable(AccessKeyDto accessKeyDto);
        int Disable(Guid key, string revoke_user);
        BindedDto GrantAppAccess(BindDto bindDto);
        BindedDto RevokeAppAccess(BindDto bindDto, string revoke_user);
    }
}