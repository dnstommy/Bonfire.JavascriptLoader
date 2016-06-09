# Bonfire.JavascriptLoader

## example

#### \App_Start

```cs
public class JavascriptLoaderConfig
{
    public static void Configure()
    {
        var configuration = new JavascriptConfiguration();

        configuration
            .EnableServerSideRendering()
            .AddFile("/Content/js/server.js");

        Initializer.Initialize(configuration);
    }
}
```

#### \_Layout.cshtml

##### Razor View

```html
<!DOCTYPE html>
<html>
  <head>
    <meta charset="utf-8">
    <title>JavascriptLoader</title>

    @Html.JavascriptLoader().Loader()
  </head>
  <body>
    @Html.JavascriptLoader().Render("App", new { title: "Hello World!" })

    <script src="client.js"></script>
    @Html.JavascriptLoader().Components()
  </body>
</html>
```

##### Rendered Output
```html
<!DOCTYPE html>
<html>
  <head>
    <meta charset="utf-8">
    <title>JavascriptLoader</title>

    <script>window.__JavascriptLoader=function(){function n(n,r,t){var u={name:n,id:r,props:t};return e?e(u):void i.push(u)}function r(n){e=n,i.reverse().reduceRight(function(n,r,t){return e(r),i.splice(t,1),r},{})}var e,i=[];return{add:n,init:r}}();</script>
  </head>
  <body>
    <div id="react_249sdh9783"></div>  

    <script src="client.js"></script>
    <script>window.__JavascriptLoader.add('App', 'react_249sdh9783', { title: 'Hello World!' });</script>
  </body>
</html>
```

#### client.js

```js
import React from 'react';
import ReactDOM from 'react-dom';
import components from './components';

// Use the exposed initilization method to
// render all the components on the screen
window.__JavascriptLoader.init((c) => {
  const element = document.getElementById(c.id);
  const component = components[c.name];

  if (!element || !component) return;

  ReactDOM.render(
    React.createElement(component, c.props || {}),
    element
  );
});
```

#### server.js

```js
import React from 'react';
import ReactDOMServer from 'react-dom/server';
import components from './components';

const render = (name, props) => {
  const component = components[name];

  if (!component) return;

  return ReactDOMServer.renderToString(React.createElement(component, props || {}));
};

export { render };
```

#### components.js

```js
import Title from './Title.jsx';

// Register components
export default {
  Title,
};
```

#### Title.jsx

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

export default App;
```
