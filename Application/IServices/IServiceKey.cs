using DataAccess.Models;
using Domain.DTOs;
using System;
using System.Collections.Generic;

namespace Application.IServices
{
    public interface IServiceKey
    {
        List<Key> GetKeys();
        Key GetKey(int id);
        AssingnedKeyDTO AssignKey(AssingnKeyDTO assignKeyDTO);
        AssingnedKeyDTO Enable(AccessKeyDTO accessKeyDTO);
        int Disable(Guid key, string revoke_user);
        BindedDTO GrantAppAccess(BindDTO bindDTO);
        BindedDTO RevokeAppAccess(BindDTO bindDTO, string revoke_user);
    }
}