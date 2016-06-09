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
