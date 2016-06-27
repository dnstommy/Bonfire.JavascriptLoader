(function () {
  var _componentsToLoad = [];
  var _renderer;

  return {
      render: render,
      init: init
  };

  function render(name, id, props) {
      var component = {
          name: name,
          id: id,
          props: props,
      };

      if (_renderer) {
          return _renderer(component);
      }

      _componentsToLoad.push(component);
  }

  function init(render) {
      _renderer = render(_componentsToLoad);
  }
})()
