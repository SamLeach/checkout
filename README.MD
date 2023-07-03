# README

## Solution Structure

There are 2 Web Api Projects:

1. ***Sam.Checkout***
This is the Gateway Payment Api (I should have named it Sam.Checkout.Web but want to finish the exercise now) The responsibilities here are HTTP concerns, model validation, response codes. The controller deligates the work to the Domain Payment Handler and Payment Query.

2. ***Sam.Checkout.FakeAcquiringBank***
This is a very simple fake Acquiring Bank that simply returns a new Guid for every new payment it receives.

Each Web Api has it's own Client in `Sam.Checkout.Clients`. These are quick and dirty clients but work for the exercise. Hopefuly they qualify as "extra mile bonus points".

The rest of the solution follows the "Domain-Infra" structure were the Domain depends on nothing and everything depends on it.

*(`PaymentCommandHandler` and `PaymentQuery` in `Sam.Checkout.Domain` project contain a lot of the logic, a lot of the other classes are boilerplate and mapping dtos and Entities)*

## Tests

There are XUnit/Moq unit tests and TestServer integration tests. Failure cases are tested. These integration tests create a "in-memory" app and test against it. It's a complete Gateway with nothing mocked except the acquring bank side.

## Security

I planned to add some db encryption but ran out of time. The apis are under HTTPS so transport is encrypted. Did not have time to do authentication either. This would be necessary in production. I want to state that anything that handles card info for real needs carful thought when it comes to security.

## Assumptions
I ended up deciding to mask the card number as it's inserted into the database. This was because the query wants a masked number anyway and there did not seem to be a requirement to ever know what the real card was. However in a real Gateway you would want to store the card in a PCI DSS zone and encrypt the column as a minimum.

## Persistance

I decided to use Entity Framework and `localdb` as a persistent datastore. In order for the Integration tests to pass, you need a localdb with access, etc. I would have liked to setup Docker but did not want to spend any more time doing it. Ideally it's dockerised. I hope setting up a persistant store gives me "extra mile bonus points". It would be easier to use Entity Framework in-memory mode.

## Notes

I left a few notes as code comments in the solution. Usually to mark that I know something was not done in the best way but usually just to mark that I did it that way in the interest of time

## Areas to improve

I ended up spending more time than I planned on so there are some areas that could be better. Dtos validation. I could have created more robust validation. It's very important. But I left some basic attributes on there.

Could have created more integration tests for other response codes. There is a trade off there as integration tests can be slow(er) to run

I put the Web level Dtos in the Domain project just to save time so I did not need to map them. I know they should be in the Web Project (`Sam.Checkout`). Hope this is ok. I am very familiar with DDD style or Clean Architecture or whatever the most trendy name for it now is

I mostly try to make my code as simple as possible or as simple as it needs to be. Although I am not sure how much I needed to "pretend" it was a real system. Although I would need more time to add all the bells and whistles.

The Dtos and Entities could have had more unseful/interesting fields on them but I decided to keep them strict to the ones mentioned in the spec.