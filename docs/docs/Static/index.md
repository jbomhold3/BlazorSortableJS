{.text-center .mt-4}
:::
# BlazorSortableJS
### Blazor library for reorderable drag-and-drop lists
### Based on SortableJS
:::

<ul class="toc col-12 col-md-5 mt-3 text-center">
	<h5>Features</h5>
	<li><a href="#simple-list-example">Simple list</a></li>
	<li><a href="#shared-lists">Shared lists</a></li>
	<li><a href="#cloning">Cloning</a></li>
	<li><a href="#disabling-sorting">Disabling sorting</a></li>
	<li><a href="#handle">Handles</a></li>
	<li><a href="#filter">Filter</a></li>
	<li><a href="#thresholds">Thresholds</a></li>
	<h5>Examples</h5>
	<li><a href="#grid-example">Grid</a></li>
	<li><a href="#nested-sortables-example">Nested sortables</a></li>
	<li><a href="#dynamic-example">Dynamic Example</a></li>
</ul>

## Features
--- 
### Simple list example

{{sample=Components/SimpleList}}

---
### Shared lists

{{sample=Components/SharedLists}}

---
### Cloning
###### Try dragging from one list to another. The item you drag will be cloned and the clone will stay in the original list.

{{sample=Components/Cloning}}

---
### Disabling Sorting
###### Try sorting the list on the left. It is not possible because it has it's sort option set to false. However, you can still drag from the list on the left to the list on the right.

{{sample=Components/DisableSorting}}

---
### Handle
{{sample=Components/Handle}}

---
### Filter
###### Try dragging the item with a red background. It cannot be done, because that item is filtered out using the filter option.

{{sample=Components/Filter}}

---
### Thresholds
###### Try modifying the inputs below to affect the swap thresholds. You can see the swap zones of the squares colored in dark blue, while the "dead zones" (that do not cause a swap) are colored in light blue.

{{sample=Components/Thresholds}}

## Examples
---

### Grid Example

{{sample=Components/GridExample}}

---

### Nested Sortables Example

{{sample=Components/Nested}}

---

### Dynamic Example

{{sample=Components/Dynamic}}