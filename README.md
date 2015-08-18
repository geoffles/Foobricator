# Foobricator
Foobricate your most awesome test data yet.

## What is it?

Have you ever found yourself in need of test data, but making that data is difficult? Introducing the **Foobricator**! 

The Foobricator is a Data Generation Tool written in C# that allows you to define a spec for your data in JSON and use it to create your data.

## So what kind of data can I Foobricate?

Just about anything - SQL statements, XML, CSV Files, you name it.

Foobricate was also designed to make multiple files which match the same data set - so maybe a list of account balances, and customer profiles that automagically agree with eachother.

The one caveat at this time is that it is very difficult to generate data with a separator between records without a trailing symbol... for now.

## How does it work?

Simply spec up your data, and foobricate! Simply drag and drop your JSON files, or use the command line:

	> Foobricate MyAwesomeSpec.json
	
Sample:

	{
		context: [{
			name:"names", type:"listSampler", source:{ type:"stringList", values:["John", "Bob", "Joe"] }
		}, {
			name:"surnames", type:"listSampler", source:{ type:"stringList", values:["Smith", "Bloggs", "Soap"] }
		}, {
			name:"randomBalance",  type:"randomDecimal", upper: 10000, lower:-10000 
		}, {
			name:"accountBalances", type:"iterator", sources:[ { 
					type:"singleValue", source: {type:"reference", refersTo:"names"} 
				}, { 
					type:"singleValue", source: {type:"reference", refersTo:"surnames"} 
				}, { 
					type:"singleValue", source: {type:"reference", refersTo:"randomBalance"} 
				}				
			]
		}],
		output:[{			
			type:"clipboardOutput",
			targets:[{				
				type:"times",
				count:1000,
				targets:[{
					type:"formatString",
					format:"<Account>\n\t<Name>{0}</Name>\n\t<Surname>{1}</Surname>\n\t<AccountBalance>${2:####0.00}</AcountBalance>\n</Account>",
					source:{type:"reference", refersTo:"accountBalances"}
				}]
			}]
		}]
	}

Pops the following onto your clipboard:

	<Account>
		<Name>Joe</Name>
		<Surname>Soap</Surname>
		<AccountBalance>$-1111.98</AcountBalance>
	</Account>
	<Account>
		<Name>Bob</Name>
		<Surname>Bloggs</Surname>
		<AccountBalance>$3535.95</AcountBalance>
	</Account>
	<Account>
		<Name>Joe</Name>
		<Surname>Smith</Surname>
		<AccountBalance>$-9856.00</AcountBalance>
	</Account>
	... and another 997 more!

You can also output directly to a file, or even multiple files!

## Licensing

Foobricator is licensed under AGPL. This is in part because of it's dependency on  Newtonsoft's JSON.net Schema. See Full text in LICENSE.txt.

Note that there is a limitation on the AGPL licensed JSON.net Schema which limits you to 1000 validations per hour. Foobricator performs one validation per input file. If you have your own license for JSON.net Schema, you are exempt from this restriction.


## List of Attributions

* Uses [Newtonsoft JSON.net](http://www.newtonsoft.com/json)
* Uses [Newtonsoft JSON.net Schema](http://www.newtonsoft.com/json)
* The documentation can compiled into HTML using [Pandoc](http://pandoc.org). It uses a template derrived from [here](https://github.com/jgm/pandoc-templates/blob/master/default.html5) (This may change).
* The HTML user doc is styled using [buttondown.css](https://gist.github.com/ryangray/1882525) 

## Build instructions

Simply checkout the source code, and build with Visual Studio. If you'd like to compile the user doc you can do so using Pandoc.