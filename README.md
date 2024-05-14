# Allowed Card Actions ASP.NET Core Microservice

## Overview

A simple microservice that returns allowed actions for a payment card based on the user id and card number.

## Table of allowed actions

| Allowed Action | Card Kind - Prepaid | Debit | Credit | Card Status - Ordered | Inactive             | Active               | Restricted | Blocked          | Expired | Closed |
|----------------|---------------------|-------|--------|-----------------------|----------------------|----------------------|------------|------------------|---------|--------|
| ACTION1        | YES                 | YES   | YES    | NO                    | NO                   | YES                  | NO         | NO               | NO      | NO     |
| ACTION2        | YES                 | YES   | YES    | NO                    | YES                  | NO                   | NO         | NO               | NO      | NO     |
| ACTION3        | YES                 | YES   | YES    | YES                   | YES                  | YES                  | YES        | YES              | YES     | YES    |
| ACTION4        | YES                 | YES   | YES    | YES                   | YES                  | YES                  | YES        | YES              | YES     | YES    |
| ACTION5        | NO                  | NO    | YES    | YES                   | YES                  | YES                  | YES        | YES              | YES     | YES    |
| ACTION6        | YES                 | YES   | YES    | YES - if PIN set      | YES - if PIN set     | YES - if PIN set     | NO         | YES - if PIN set | NO      | NO     |
| ACTION7        | YES                 | YES   | YES    | YES - if PIN not set  | YES - if PIN not set | YES - if PIN not set | NO         | YES - if PIN set | NO      | NO     |
| ACTION8        | YES                 | YES   | YES    | YES                   | YES                  | YES                  | NO         | YES              | NO      | NO     |
| ACTION9        | YES                 | YES   | YES    | YES                   | YES                  | YES                  | YES        | YES              | YES     | YES    |
| ACTION10       | YES                 | YES   | YES    | YES                   | YES                  | YES                  | NO         | NO               | NO      | NO     |
| ACTION11       | YES                 | YES   | YES    | NO                    | YES                  | YES                  | NO         | NO               | NO      | NO     |
| ACTION12       | YES                 | YES   | YES    | YES                   | YES                  | YES                  | NO         | NO               | NO      | NO     |
| ACTION13       | YES                 | YES   | YES    | YES                   | YES                  | YES                  | NO         | NO               | NO      | NO     |

## Implementation Notes

I decided not to add `WebApi.Tests` (unit tests for the WebApi) because there is so little logic in the WebApi project
and it's tested by the integration tests quickly enough.

I separated business logic to a separate project to keep the logic not dependent on any of the web dependencies.
It is usually a good practice to separate a project like that.

I added retry logic when calling ICardService and ICardActionsService to make it more look more like a production code.
Assumption was that those services would either call to a DB (internal to the microservice or external, doesn't matter)
or an external service.

There is no healthcheck endpoint and the logging is basic, but I assume this is out of scope for this task.