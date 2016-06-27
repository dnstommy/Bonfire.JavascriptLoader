/*global __JavascriptLoader*/
import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';
import components from './components';

__JavascriptLoader.init((componentsToLoad) => {
  componentsToLoad.reverse().reduceRight((prev, c, idx) => {
    render(c);

    componentsToLoad.splice(idx, 1);
  }, {});

  return render;
});

function render(component) {
  const componentFn = components[component.name];

  if (!componentFn) return;

  if (__JavascriptLoader.isServerSide) {
    return ReactDOMServer.renderToString(
      React.createElement(componentFn, component.props || {})
    );
  }

  ReactDOM.render(
    React.createElement(componentFn, component.props || {}),
    document.getElementById(component.id)
  );
}
