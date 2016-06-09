(function () {
  var _componentsToLoad = [];
  var _render;

  return {
      add: add,
      init: init
  };

  function add(name, id, props) {
      var component = {
          name: name,
          id: id,
          props: props,
      };

      if (_render) {
          return _render(component);
      }

      _componentsToLoad.push(component);
  }

  function init(render) {
      _render = render;
      _componentsToLoad.reverse().reduceRight(function (prev, component, idx) {
          _render(component);
          _componentsToLoad.splice(idx, 1);

          return component;
      }, {});
  }
})()
