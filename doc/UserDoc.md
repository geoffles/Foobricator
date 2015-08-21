#  Foobricator User Documentation {#Documentation}

##  Invocation {#Invocation}

Foobricate supports arguments for the input files as well as well as switches.

The order of arguments does not matter.

	foobricate.exe [template1.json, template2.json, ...] [/DebugLevelWarning:<true,false>]

You can also drag and drop a template file onto the EXE in an explorer window, and the program will execute it.

##  Template File Syntax {#TemplateFileSyntax}

The template files are a JSON syntax and are validated against a schema.

###  Concepts {#Concepts}
The generator works in two phases. The first is to create a *context* for data generation. This is simply the sources which will be used to generate the data.

The second phase is the output phase, in which the sources from the context are fed into an output format. 

###  Basic Format {#BasicFormat}
The template file must start with the following structure:

	{
		context: [],
		output: []
	}

The elements of the context array must simply be [Source](/Sources/) elements, and the elements of the output must be [Root Ouput](/Root Output/) elements.

####  References {#References}

Elements in the context can be named and referenced by other context elements and outputs.

The name is indicated by the *name* property of an object. For example:

	{
		name:"sampleList", 
		type:"stringList",
		values:[...]
	}

The above defines a list of strings that can be referenced as a source element later as such:

	{
		type:"listSampler", 
		source:{type:"reference",
		refersTo:"sampleList"}
	}

The above will define a list sampler which selects random values from the sample list.


#### Iterator Scopes
Iterator scopes are a mechanism for partitioning the steps of data generation. An iterator is a captured set of generated values, and new values are "pulled" by a *times* element when it loops over the scopes that an iterator is subscribed to.

Order of execution between scopes is not guaranteed and is controlled by the nesting of your *times* elements.

----------

#  Sources {#Sources}

This is the list of sources available

----------


##  StringList {#StringList}

----------

The string list is a base level data tool. It allows you to define a list of strings which can then be sampled from to generate your rows.

**Required Properties**

*  type: Must be "stringList"
*  values: Must be an array of strings.

**Optional Properties**

*  name: Must be a string.


**Example**

	{
		name:"listFoo", 
		type:"stringList", 
		values:["Foo", "Bar", "Baz"]
	}

----------

## Reference {#Reference}

A reference allows you to refer to a previously declared source by the name declared on that source in the context.

**Required Properties**

*  type: Must be "reference"
*  refersTo: Must be string that points to a named source.

**Optional Properties**

None


**Example**

	{
		type:"reference",
		refersTo:"listFoo"
	}

----------


##  ListSampler {#ListSampler}

The list sampler allows you to select a random item from a list (eg: StringList. The list can either be referenced, or declared directly in the source property.

**Required Properties**

* type: Must be "listSampler"
* source: Must be a valid list object or refer to one.

**Optional Properties**

* name: Must be a string

**Example**

	{
		name:"sampleFoo",
		type:"listSampler",
		source:{type:"stringList",
		values: ["Foo", "Bar", "Baz"]
	}

Or

	{
		name:"sampleFoo",
		type:"listSampler",
		source:{type:"reference", refersTo:"listFoo"}
	}

----------

##  Iterator {#Iterator}

The Iterator is a *capture* tool. In order to understand how and why to use the Iterator, we must understand how data is generated.

The Data Generator will select random values from sources like [ListSampler](#ListSampler), [RandomInt](#RandomInt), [RandomDate](#RandomDate), and [RandomDecimal](#RandomDecimal) each time they are interrogated by the output.

The Iterator will also interrogate these items, but will then store that value and return the same value each time. This is useful if you need to **reuse** a value.

The Iterator will interrogate it's sources on declaration and when inside a looping output such as [Times](#Times). All iterators are evaluated in order of declaration, so iterators can safely reference a previously declared iterator, however scope evaluation order is not guaranteed (see below).

Iterators can be *scoped*. The iterators will only be iterated by a [Times](#Times) with a matching scope. The scope property is optional and defaults to `Global`. You can subscribe to multiple scopes by using a `:` between scope names. Similarly the times can evaluate multiple scopes in the same way. The order of scope execution is not guaranteed - to do this, you need to structure your nested *times* elements correctly.

The iterator accepts an array of sources, which can either be references or directly declared sources.

**Note:** The Iterator is the *prefered* source for the [FormatString](#FormatString) output.

If you iterator has sources which return lists, it will flatten the lists in order to ultimately produce only 1 list.
 

**Required Properties**

* type: Must be "iterator"
* sources: Must be an array of Sources

**Optional Properties**

* name: Must be a string
* scope: Must be a string


**Example**

	{
		name:"sampleIterator", 
		type:"iterator", 
		scope: "foo",
		sources:[
			{type:"stringList", values: ["Foo", "Bar", "Baz"]},
			{type:"reference", refersTo:"listFoo"}
			]
	}


----------

##  Switch {#Switch}

Switch is a mapping tool that allows for values to be mapped to other values. This is useful if you need to produce two values in the data set that are related to eachother.

You need to define the map for a switch that is an array of objects containing a key and a value.

**Required Properties**

* type: Must be "switch"
* source: Any source type or reference
* map: An array of objects like `{key:"foo", value:"bar"}`

**Optional Properties**

* name: Must be a string


**Example**

	{
		name:"sampleSwitch", 
		type:"switch", 
		map: [ {key:"Foo", value:"Fifi"}, {key:"Bar", value:"Baba"} ],
		source:{type:"stringList", values: ["Foo", "Bar"]}
	}

----------

##  SingleValue {#SingleValue}

Single value selects the value of a source as a single value object - such as a string or a number. This is typically used for creating a final iterator to be passed into a format string (see [Sample Input](/Sample Input/), and is best understood in terms of it's contrast to [Tuple Value](/TupleValue/). Single value will only return a value if there is only 1 value from what the source returns.

**Required Properties**

* type: Must be "singleValue"
* source: Any source type or reference

**Optional Properties**

* name: Must be a string

**Example**

	{ 
		type:"singleValue",
		source:{
			type:"reference",
			refersTo:"sampleIterator"
		} 
	}

----------

## TupleValue {#TupleValue}

Tuple value allows you to select an item of a list of items by it's index. For example you may wish to select a specific item from an [Iterator](/Iterator/).

**Required Properties**

* type: Must be "tupleValue"
* source: Any source type or reference
* index: An integer referring to the 0 based index of the item you would like 

**Optional Properties**

* name: Must be a string

**Example**

	{ 
		type:"tupleValue",
		index:2,
		source: {
			type:"reference", 
			refersTo:"sampleIterator"} 
	}

----------

## RandomDate {#RandomDate}

The random date tool allows you to define a base date and the amount of days it can vary from the base.

**Required Properties**

* type: Must be "randomDate"
* baseDate: Must be "baseDate" and in the format "YYYY-MM-DD"
* rangeUp: Must be an integer

**Optional Properties**

* Name: Must be a string


**Example**

	{
		name: "someRandomDate",
		type:"randomDate",
		baseDate:"2015-01-01",
		rangeUp:2
	}


----------

## RandomInt {#RandomInt}

The random int tool lets you specify an upper (exclusive) and lower (inclusive) bound from which random integers can be generated.

**Required Properties**

* type: Must be "randomInt"
* upper: Must be "upper" and be an Integer
* lower: Must be "lower" and be an Integer

**Optional Parameters**

* Name: Must be a string

**Example**

	{
		name: "someRandomInt", 
		type:"randomInt", 
		upper:5, lower:2
	}

----------

## RandomDecimal {#RandomDecimal}

The random decimal tool lets you specify an upper (exclusive) and lower (inclusive) bound from which random decimals can be generated with a precision of 28 - 29 significant digits.

**Required Properties**

* type: Must be "randomDecimal"
* upper: Must be "upper" and be an Integer
* lower: Must be "lower" and be an Integer

**Optional Parameters**

* Name: Must be a string

**Example**

	{
		name: "someDecimal",
		type:"randomDecimal",
		upper:20, 
		lower:10
	}

**Please Note**

The decimal precision can be set on the output string


----------

##PadLeft {#PadLeft}

The pad left tool is used to pad a string on the left with the given character.

**Required Properties**

* type: Must be "padLeft"
* source:
    * type: Must be "reference"
    * refersTo: Must be the *name* of the source item
* length: Must be "length" and an integer
* character: Must be "character" and a *single* character

**Optional Parameters**

* Name: Must be a string

**Example**

	{ 
		name: "paddedOnLeft",
		type:"padLeft",
		source:{type:"reference", refersTo:"someDecimal"}, 
		length: 42, 
		character: "#"
	}

----------

## SubString {#SubString}


The substring tool allows you to copy out a piece of a string from a specified location.

**Required Properties**

* type: Must be "subString
* start: Must be a non negative integer
* length: Must be an integer greater than 0
* source:
    * type: Must be "reference"
    * refersTo: Must be the *name* of the source item

**Optional Parameters**

* Name: Must be a string

**Example**

	{
		name: "wordsSubStr",
		type:"subString",
		start:1,
		length:1,
		source:{type:"reference", refersTo:"words"}
	}

----------

#Root Outputs {#RootOutputs}

----------
## Times

[Times](#Times) can be used as a root output as well. Separator is not supported when used as a root.

This allows you to use a common data set across two ouptut files.  

##ClipboardOutput {#ClipboardOutput}

The clipboard output will place your generated data directly onto your clipboard. It can then be pasted into your editor of choice.

**Required Properties**

* type: Must be "clipboardOutput"
* targets: Must be an array of type "output" with at least one or more entries

**Optional Properties**

None.

**Example**

    {
        type:"clipboardOutput",             
        targets:[{                
            type:"times", count: 100, targets: [
                {
                    type: "formatString", 
                    format:"{0}|{1:yyyy-MM-dd}|{2,-5:000}|{3,10:00000.00#}|{4}|{5}|{6}|{7}|", 
                    source:{type: "reference", refersTo: "outputIt"}
                }, {
                    type:"conditionalOutput", 
                    target:[{
                        type:"formatString",
                        format:"{0}",
                        source:{type: "reference", refersTo: "outputIt"}
                    }], 
                    when: {
                        source:{type:"reference", refersTo:"myListIterator"}, 
                        op:"NotEq", 
                        rightHandSide:"b"
                    }
                }
            ]
        }]
    }


----------

##FileOutput {#FileOutput}

File output is used to directly output your generation data to a file.

You can ask it not to overwrite the file by specifying `append:true`. This is useful for applying headers and footers. You can do this by declaring two *fileOutput*s that write to the same file, with the second output specifying `append:true` (see example).

**Required Properties**

* type: Must be "fileOutput"
* filename: Must be a string
* targets: must be an array of type "output" with at least one or more entries

**Optional Properties**

* append: Must be true or false 

**Example**

    output:[{
			type: "fileOutput",
	        fileName: "myGeneratedDataFile.txt",
			append: true,
	        targets: [{
				type:"literal",
				value:"HEADER\N"
			}]
		},{
	        type: "fileOutput",
	        fileName: "myGeneratedDataFile.txt",
			append: true,
	        targets: [{
	            type: "times",
	            count: 100,
	            targets: [
	                {
	                    type: "formatString", 
	                    format:"{0}|{1:yyyy-MM-dd}|{2,-5:000}|{3,10:00000.00#}|{4}|{5}|{6}|{7}|", 
	                    source:{type: "reference", refersTo: "outputIt"}
	                }, {
	                    type:"conditionalOutput", 
	                    target:[{
	                        type:"formatString",
	                        format:"{0}",
	                        source:{type: "reference", refersTo: "outputIt"}
	                    }], 
	                    when: {
	                        source:{type:"reference", refersTo:"myListIterator"}, 
	                        op:"NotEq", 
	                        rightHandSide:"b"
	                    }
	                }
	            ]
	        }]
	    }
	]

----------


#  Outputs {#Outputs}

This is a listing of the available outputs. These must all be placed under a Root Output

----------

##  Literal

A literal is the most basic output which simply outputs a literal value.

**Required Properties**

* type: Must be "literal"
* value: Must be a string

**Optional Parameters**

None
 
**Example**

	{
		type: "literal",
		value:"Foobricate"		 
	}


----------

##  FormatString {#FormatString}


Format string allows you to output source items into an arbitrary string with formatted substitions including number padding, date formatting, and fixed width columns.

By default, format string will emit a new line at the end. To suppress this, specify `suppressEndLine:false`.

Format string canbe used in two ways: Rich and Plain formatting.

### Plain Formatting

Plain formatting must specify a `source` object and can use only a [Reference](#Reference) object for it's source and the format can be specfied either as a string or an array of strings.

When you use an array of strings, they will all be joined together as if they were all one string.

The format string uses exactly the same rules as the [.Net formatstring](http://blogs.msdn.com/b/kathykam/archive/2006/03/29/564426.aspx). The basics are as follows:

If we have an iterator with 3 elements in, say:
*  John
*  Smith
*  500

We can use a format string like this:

	<Name>{0}</Name>
	<Surname>{1}</Surname>
	<AccountBalance>{2}</AccountBalance>

Which would then ouptut:

	<Name>John</Name>
	<Surname>Smith</Surname>
	<AccountBalance>500</AccountBalance>

Or we could use some advanced formatting options to produce a fixed column output:

	{0,12}|{1,12}|{2:000000.00}

Which produces:

	John        |Smith        |000500.00


**Required Properties (Plain formats)**

* type: Must be "formatString"
* format: Must be a string or array of strings.
* source: Must be a reference.

**Required Properties (Rich formats)**

* type: Must be "formatString"
* format: Rich Formatted array, items should be
	* string
	* value object (see example).


**Optional Parameters**

* suppressEndLine: Must be a boolean.
 
**Example**

Plain:

	{
		type: "formatString",
		format:"{0,12}|{1,12}|{2:000000.00}", 
		source: { type:"reference", refersTo:"sampleIterator" } 
	}

	{
		type:"formatString",
		format: [ 
			"{0,12}|", 
			"{1,12}|",
			"{2:000000.00}"
		],
		source: { type:"reference", refersTo:"sampleIterator" }
	}

Rich:

	{
		type:"formatString",
		format: [ 
			"<FooFoo>", 
			{
				value:{
					type:"singleValue",
					source:{
						type:"reference",
						refersTo:"sampleIterator"
					}
				},
				format:",10:##00.00"
			},
			"</FooFoo>"
		]		
	}

----------


##  ConditionalOutput {#ConditionalOutput}

ConditionalOutput allows you to only output it's targets when a condition is met.

Conditional output uses a [When](#When) object to encapsulate the condition.

**Required Properties**

* type: Must be "conditionalOutput"
* when: Must be a [When](#When) object
* target: Must be and array of outputs    

**Optional Parameters**

None
 
**Example**

	{
		type: "conditionalOutput",
		when: { 
			source:{type:"reference", refersTo:"sampleIterator"}, 
			op:"eq", 
			rightHandSide:"1"
		},
		target:{
			type: "formatString",
			format: "FirstItem:{0}",
			source: {type:"reference", refersTo:"rowIterator"}
		} 
	}


### When {#When}

The when is not actually an output or a source, and can only be used by the [ConditionalOutput](#ConditionalOutput) (at this stage).

**Comparison Operations:** The *op* property indicates the type of comparison. The available options are `Eq`,`Lt`,`Gt` and `Not`. `Eq`,`Lt` and `Gt` work together in an "or" fashion whilst `Not` performs a logical not on the entire expression. For example `GtEq` is "greater than, or equal to", whilst `NotLtEq` is "not less than AND not equal to".   

**Source and RightHandSide:** The *type* for *source* and *rightHandSide* must be the same. This means that if your source is a string (say an iterator from a listSampler that samples a stringList), your right hand side must also be a string.  

**Required Properties**

* source: Must be a source or reference object
* op: Must be a string. Can be composed of any of `Not`,`Eq`,`Lt`,`Gt`.
* rightHandSide: Must be a type matching the type from source.    

**Optional Parameters**

None
 
**Example**

	{
		type: "conditionalOutput",
		when: { 
			source:{type:"reference", refersTo:"sampleIterator"}, 
			op:"eq", 
			rightHandSide:"1"
		},
		target:{
			type: "formatString",
			format: "FirstItem:{0}",
			source: {type:"reference", refersTo:"rowIterator"}
		} 
	}

----------

##  Times

The times output is a repeater. The times output will also iterate all iterators with a match *scope* on each repeat. Iterators are iterated in order of declaration. Each target will be evaluated once per iteration and in total the Count of times.

You can control which iterators are iterated by using the the optional property *scope*. If no scope is defined, it defaults to `Global`. See [Iterator](#Iterator) for more information.

You can supply a separator which will be emitted to the output between each output. This is useful for supplying the commas between records in JSON for example.

**Required Properties**

* type: Must be "times"
* count: Must be an integer greater than zero
* targets: Must be an array of targets for output    

**Optional Parameters**

* separator: Must be a string
* scope: Must be a string
 
**Example**

	{
		type: "times",
		count: 100,
		separator: ":",
		scope: "foo",
		targets: [{
			type: "formatString",
			format: "FirstItem:{0}",
			source: {type:"reference", refersTo:"rowIterator"}
		}]
	}

----------


## Literal

A literal is simply a literal value in the output. What ever is in *value* will be output. 

This is allows you to define header and trailer information, such as root noodes for XML for example. 


**Required Properties**

* type: Must be "literal"
* value: Must be a string    

**Optional Parameters**

None
 
**Example**

	{
		type: "literal",
		value: "<AllCustomers>"
	}