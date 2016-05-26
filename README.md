# Bonfire.React

## example

#### \_Layout.cshtml

##### Razor View

```html
<!DOCTYPE html>
<html>
  <head>
    <meta charset="utf-8">
    <title>React</title>

    @Html.React().InitLoaderJavascript()
  </head>
  <body>
    @Html.React().Render("App", new { title: "Hello World!" })

    <script src="main.js"></script>
    @Html.React().InitComponentsJavascript()
  </body>
</html>
```

##### Rendered Output
```html
<!DOCTYPE html>
<html>
  <head>
    <meta charset="utf-8">
    <title>React</title>

    <script>window.__ReactLoader=function(){function n(n,r,t){var u={name:n,id:r,props:t};return e?e(u):void i.push(u)}function r(n){e=n,i.reverse().reduceRight(function(n,r,t){return e(r),i.splice(t,1),r},{})}var e,i=[];return{add:n,init:r}}();</script>
  </head>
  <body>
    <div id="react_249sdh9783"></div>  

    <script src="main.js"></script>
    <script>window.__ReactLoader.add('App', 'react_249sdh9783', { title: 'Hello World!' });</script>
  </body>
</html>
```

#### main.js

```js
import React from 'react';
import ReactDOM from 'react-dom';

import app from './app';

// Register components
const components = {
  ...app
};

// Use the exposed initilization method to
// render all the components on the screen
window.__ReactLoader.init((c) => {
  const element = document.getElementById(c.id);
  const component = components[c.name];

  if (!element || !component) return;

  ReactDOM.render(
    React.createElement(component, c.props || {}),
    element
  );
});
```

#### app.js

```js
import React from 'react';

class App extends React.Component {
  static propTypes = {
    title: React.PropTypes.string.isRequired,
  }

  render() {
    return (
      <h1>{this.props.title}</h1>
    );
  }
}

export App;
```
