/*global __JavascriptLoader*/
//import React from 'react';
//import ReactDOM from 'react-dom';
//import ReactDOMServer from 'react-dom/server';
//import components from './components';

__JavascriptLoader.init((componentsToLoad) => {
  componentsToLoad.reverse().reduceRight((prev, c, idx) => {
    render(c);

    componentsToLoad.splice(idx, 1);
  }, {});

  return render;
});

function render(component) {
  //const componentFn = components[component.name];

  //if (!componentFn) return;

  if (__JavascriptLoader.isServerSide) {
      return vueRender();
  }

  //ReactDOM.render(
  //  React.createElement(componentFn, component.props || {}),
  //  document.getElementById(component.id)
  //);
}

function vueRender() {

    process.env.VUE_ENV = 'server';

    const fs = require('fs');
    const path = require('path');

    const filePath = 'C:\BB\Loader\src\Bonfire.JavascriptLoader.Demo\Assets\Vue\server.js';
    const code = fs.readFileSync(filePath, 'utf8');

    const bundleRenderer = require('vue-server-renderer').createBundleRenderer(code);


    return new Promise(function (resolve, reject) {
        
        bundleRenderer.renderToString(params.data, (err, resultHtml) => { // params.data is the store's initial state
            if (err) {
                reject(err.message);
            }
            resolve({
                html: resultHtml,
                globals: {
                    data: params.data 
                    /*
                        public IActionResult Index()
                        {
                            // "data" comes from the .NET View Controller
                            return View(new { Something = "This came from .NET Controller before the View loaded" }); 
                        }
                    */
                }
            });
        });
    });

}
