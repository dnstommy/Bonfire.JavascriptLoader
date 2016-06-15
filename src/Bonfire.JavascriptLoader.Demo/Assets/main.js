import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';
import components from './components';

if (global.__JavascriptLoader && global.__JavascriptLoader.init) {
  global.__JavascriptLoader.init((c) => {
    const element = document.getElementById(c.id);
    const component = components[c.name];

    if (!element || !component) return;

    ReactDOM.render(
      React.createElement(component, c.props || {}),
      element
    );
  });
}

const render = (name, props) => {
    const component = components[name];

    if (!component) return;

    return ReactDOMServer.renderToString(React.createElement(component, props || {}));
};

export { render };
