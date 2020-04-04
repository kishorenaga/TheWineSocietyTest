Feature: User can buy wines from wine society

@e2e @RefineSearch
Scenario Outline: User can search for wines in the product list page
	Given I am on the products listing page
	When I search for wines with '<Name>' using the refine search 
	Then I see the '<WineName>', '<Country>', '<Description>', '<BottlePrice>', '<BottleCasePrice>' of the wine 
Examples: 
| Name      | WineName                | Country | Description                                                                   | BottlePrice | BottleCasePrice |
| Gavi 2019 | The Society's Gavi 2019 | Italy   | Gavi is a great go-to white wine. Fresh crisp and dry with lemon and green... | £35.00      | £179.00         |

@e2e @SortBy @LowtoHigh
Scenario Outline: User can sort product list by price Low to High 
	Given I am on the products listing page
	When I search for wines with '<Name>' using the refine search
	When I '<Sort>' the wine list by price Low to High 
	Then I see wine listing ordered by price Low to High
Examples: 
		| Name | Sort        |
		| 2018 | Low TO High |

@e2e @SortBy @AToZ
Scenario Outline: User can sort product list by price A to Z 
	Given I am on the products listing page
	When I search for wines with '<Name>' using the refine search
	When I '<Sort>' the wine list by price Low to High 
	Then I see wine listing ordered by A to Z
Examples: 
		| Name | Sort |
		| 2018 | A-Z  |
	
@e2e @ProducerDetails
Scenario Outline: User can see the producer profile 
	Given I am on the products listing page
	When I search for wines with '<Name>' using the refine search
	And I select a product to see the producer details 
	Then I see the '<ProducerName>', '<Description>' and Avatar
	Examples: 
	| Name                              | ProducerName | Description                                                                                                                                                                                                              |
	| The Society's Côtes-du-Rhône 2018 | Feudo Arancio | This substantial Sicilian estate is based on the windy south coast of Sicily. It has two large vineyard areas, covering almost 700 hectares in total, situated at Sambuca di Sicilia in the west and Ragusa in the east. |

@e2e @ProductDetails
Scenario Outline: User can see the product details in product description page
	Given I am on the products listing page
	When I search for wines with '<Name>' using the refine search
	And I select a product to see the product details 
	Then I see the '<ProductName>', '<BottlePrice>', '<CasePriceFor12>', '<CasePriceFor6>' and '<Description>'
	And I see the Product Image
	Examples: 
	| Name     | ProductName                                        | BottlePrice | CasePriceFor6 | CasePriceFor12 | Description                                                                                                                                                                                                                                                            |
	| Sancerre | Sancerre ‘La Reine Blanche', Domaine Vacheron 2018 | £9.00       | £38.00       | £120.00         | A stunning, fruit-packed wine with easy charm. The name was conjured up because this wine is sourced from both banks of the Rhône, with the grenache coming mainly from the left (gauche) and the syrah from the village of Daumazan (on the rive droite), near Tavel. |

@e2e @OutOfStock
Scenario Outline: User can see the details of alternative product when the actual product is out of stock
	Given I am on the products listing page
	When I search for wines with '<Name>' using the refine search
	And I select a out of stock product to see the details 
	Then I see out of stock product in the product details page
	And I see the Alternative product dispalyed with '<AlternativeProducerName>', '<AlternativeShortDescription>' and Image
	Examples: 
	| Name        | AlternativeProducerName | AlternativeShortDescription                                                   |
	| Spanish Red | The Society's Gavi 2019 | Gavi is a great go-to white wine. Fresh crisp and dry with lemon and green... |