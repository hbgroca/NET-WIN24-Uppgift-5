﻿using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public class MemberFactory
{
    public static MemberEntity Create(AddMemberFormModel form)
    {
        return new MemberEntity
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            ImageUrl = form.ImageName,
            PhoneNumber = form.Phone,
            Title = form.Title,
            Status = "Active",
            BirthDate = DateOnly.Parse($"{form.Year}-{form.Month}-{form.Day}"),
        };
    }

    public static MemberEntity Create(MemberSignUpFormModel form)
    {
        var entity = new MemberEntity();
        entity.UserName = form.Email;
        entity.FirstName = form.FirstName;
        entity.LastName = form.LastName;
        entity.Email = form.Email;

        entity.ImageUrl = "/images/defaultmember.png";
        entity.Title = "Junior";
        entity.Status = "Active";

        entity.DateCreated = DateTime.Now;
        entity.DateUpdated = DateTime.Now;

        entity.PhoneNumber = form.PhoneNumber;
        entity.BirthDate = DateOnly.Parse($"{form.Year}-{form.Month}-{form.Day}");

        return entity;
    }

    public static MemberModel Create(MemberEntity entity)
    {
        return new MemberModel
        {
            Id = Guid.Parse(entity.Id),
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email!,
            ImageUrl = entity.ImageUrl,
            Phone = entity.PhoneNumber!,
            DateCreated = entity.DateCreated,
            DateUpdated = entity.DateUpdated,
            Title = entity.Title,
            Status = entity.Status,
            BirthDate = entity.BirthDate,
            Address = AddressFactory.Create(entity.Address!),
        };
    }

    public static MemberModel CreateWithProjects(MemberEntity entity)
    {
        var model = new MemberModel();
        model.Id = Guid.Parse(entity.Id);
        model.FirstName = entity.FirstName;
        model.LastName = entity.LastName;
        model.Email = entity.Email!;
        model.ImageUrl = entity.ImageUrl;
        model.Phone = entity.PhoneNumber!;
        model.DateCreated = entity.DateCreated;
        model.DateUpdated = entity.DateUpdated;
        model.Title = entity.Title;
        model.Status = entity.Status;
        model.BirthDate = entity.BirthDate;
        model.Address = AddressFactory.Create(entity.Address!);
        model.Projects = entity.Projects.Select(ProjectFactory.Create).ToList();

        return model;
    }

    public static MemberEntity Create(MemberModel model)
    {
        return new MemberEntity
        {
            Id = model.Id.ToString(),
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            ImageUrl = model.ImageUrl,
            PhoneNumber = model.Phone,
            Title = model.Title,
            Status = model.Status,
            DateUpdated = model.DateUpdated,
            DateCreated = model.DateCreated,
            BirthDate = model.BirthDate,
            Address = AddressFactory.Create(model.Address),
            Projects = model.Projects.Select(ProjectFactory.Create).ToList()
        };
    }

    public static MemberEntity Update(EditMemberFormModel form, MemberEntity member)
    {
        member.FirstName = form.FirstName;
        member.LastName = form.LastName;
        member.Email = form.Email;
        member.PhoneNumber = form.Phone;
        member.BirthDate = DateOnly.Parse($"{form.Year}-{form.Month}-{form.Day}");
        member.Title = form.Title;
        member.Status = form.Status;

        if (!string.IsNullOrEmpty(form.ImageName))
            member.ImageUrl = form.ImageName;

        return member;
    }
}
