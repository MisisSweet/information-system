Feature: Algorithm
	I want to be sure that algorithm CRUD is working correctly.

Background:
	Given I have a module "Test_Module" without description
	And I have an algorithm registration "Registration1" with following parameters:
		| name          | description | template   | location | storageLink      | modelName | fileName | sourceType |
		| Registration1 |             | RuleMetric | S3       | s3bucketTest.txt | test      |          | None       |

Scenario: Get algorithm
	Given I have an algorithm "Algorithm1" with state Active and description "My first Algorithm"
	When I'm getting the algorithm "Algorithm1"
	Then The system returns algorithm "Algorithm1" with state Active and description "My first Algorithm"

Scenario: Create algorithm without description
	When I'm creating an algorithm "Algorithm1" with state Inactive without description
	And I'm getting the algorithm "Algorithm1"
	Then The system returns algorithm "Algorithm1" with state Inactive without description

Scenario: Create algorithm with description
	When I'm creating an algorithm "Algorithm1" with state InSimulation and description "My first Algorithm"
	And I'm getting the algorithm "Algorithm1"
	Then The system returns algorithm "Algorithm1" with state InSimulation and description "My first Algorithm"

Scenario: Create a duplicate algorithm
	Given I have an algorithm "Algorithm1" with state Active and description "My first Algorithm"
	When I'm creating an algorithm "Algorithm1" with state Active and description "My first Algorithm"
	Then the system returns bad request error
	And the error message contains field key "name"
	And the error message contains field key "systemName"
	And the error message text is "This field should be unique."

Scenario: Update algorithm
	Given I have an algorithm "Algorithm1" with state Inactive without description
	When I'm updating an algorithm "Algorithm1" with state Active and description "My first Algorithm"
	And I'm getting the algorithm "Algorithm1"
	Then The system returns algorithm "Algorithm1" with state Active and description "My first Algorithm"

Scenario: Delete algorithm
	Given I have an algorithm "Algorithm1" with state Active without description
	When I'm deleting an algorithm "Algorithm1"
	Then the system does not contain the algorithm "Algorithm1" anymore

Scenario: Get inexistent algorithm
	Given I have an algorithm "Algorithm1" with state Active without description
	When I'm getting the algorithm "Algorithm2"
	Then the system returns an error