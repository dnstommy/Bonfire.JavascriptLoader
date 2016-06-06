import React from 'react';
import ReactDOMServer from 'react-dom/server';
import components from './components';

// Use the exposed initilization method to
// render all the components on the screen
window.__JavascriptLoader.init((c) => {
  const element = document.getElementById(c.id);
  const component = components[c.name];

  if (!element || !component) return;

  ReactDOMServer.renderToString(
    React.createElement(component, c.props || {}),
    element
  );
});
