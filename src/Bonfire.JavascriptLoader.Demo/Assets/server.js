import React from 'react';
import ReactDOMServer from 'react-dom/server';
import components from './components';

const render = (name, props) => {
  const component = components[name];

  if (!component) return;

  return ReactDOMServer.renderToString(React.createElement(component, props || {}));
};

export { render };
