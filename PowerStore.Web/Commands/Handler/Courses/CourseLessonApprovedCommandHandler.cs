﻿using PowerStore.Domain.Courses;
using PowerStore.Services.Courses;
using PowerStore.Web.Commands.Models.Courses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace PowerStore.Web.Commands.Handler.Courses
{
    public class CourseLessonApprovedCommandHandler : IRequestHandler<CourseLessonApprovedCommand, bool>
    {
        private readonly ICourseActionService _courseActionService;

        public CourseLessonApprovedCommandHandler(ICourseActionService courseActionService)
        {
            _courseActionService = courseActionService;
        }

        public async Task<bool> Handle(CourseLessonApprovedCommand request, CancellationToken cancellationToken)
        {
            var action = await _courseActionService.GetCourseAction(request.Customer.Id, request.Lesson.Id);
            if (action == null)
            {
                await _courseActionService.InsertAsync(new CourseAction() {
                    CustomerId = request.Customer.Id,
                    CourseId = request.Course.Id,
                    LessonId = request.Lesson.Id,
                    Finished = true
                });
            }
            else
            {
                action.Finished = true;
                await _courseActionService.Update(action);
            }
            return true;
        }
    }
}
