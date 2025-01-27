﻿using FluentValidation;
using PowerStore.Core.Validators;
using PowerStore.Services.Localization;
using PowerStore.Web.Areas.Admin.Models.Courses;
using System.Collections.Generic;

namespace PowerStore.Web.Areas.Admin.Validators.Courses
{
    public class CourseLessonValidator : BasePowerStoreValidator<CourseLessonModel>
    {
        public CourseLessonValidator(
            IEnumerable<IValidatorConsumer<CourseLessonModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Courses.Course.Lesson.Fields.Name.Required"));
            RuleFor(x => x.CourseId).NotEmpty().WithMessage(localizationService.GetResource("Admin.Courses.Course.Lesson.Fields.CourseId.Required"));
            RuleFor(x => x.SubjectId).NotEmpty().WithMessage(localizationService.GetResource("Admin.Courses.Course.Lesson.Fields.SubjectId.Required"));
        }
    }
}