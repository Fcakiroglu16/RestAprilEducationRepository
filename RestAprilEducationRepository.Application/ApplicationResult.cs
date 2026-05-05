using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace RestAprilEducationRepository.Application
{
    // Success => no content => 204 No Content
    // Success =>  content => 200 OK / 201 Created
    // Fail =>  no content =>  / 404 Not Found
    // Fail =>  error content => 400 Bad Request / 500 Internal Server Error

    public class ApplicationResult
    {
        public ProblemDetails? Problem;

        public bool IsSuccess => Problem is null;

        [JsonIgnore] public HttpStatusCode HttpStatusCode { get; set; }

        // 200 OK
        // 2001 Created
        // 204 No Content
        public static ApplicationResult Success()
        {
            return new ApplicationResult();
        }

        public static ApplicationResult Failure(ProblemDetails problem, HttpStatusCode status)
        {
            return new ApplicationResult()
            {
                Problem = problem,
                HttpStatusCode = status
            };
        }

        public static ApplicationResult Failure(string title, HttpStatusCode status)
        {
            return new ApplicationResult()
            {
                Problem = new ProblemDetails()
                {
                    Title = title,
                    Status = status.GetHashCode()
                },
                HttpStatusCode = status
            };
        }
    }

    public class ApplicationResult<T> : ApplicationResult
    {
        public T? Data { get; set; }

        public new static ApplicationResult<T> Success(T data, HttpStatusCode status = HttpStatusCode.OK)
        {
            return new ApplicationResult<T>()
            {
                Data = data,
                HttpStatusCode = status
            };
        }

        public new static ApplicationResult<T> Failure(ProblemDetails problem, HttpStatusCode status)
        {
            return new ApplicationResult<T>()
            {
                Problem = problem,
                HttpStatusCode = status
            };
        }

        public new static ApplicationResult<T> Failure(string title, HttpStatusCode status)
        {
            return new ApplicationResult<T>()
            {
                Problem = new ProblemDetails()
                {
                    Title = title,
                    Status = status.GetHashCode()
                },
                HttpStatusCode = status
            };
        }
    }
}
