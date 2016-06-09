import React from 'react';

class Title extends React.Component {
  static propTypes = {
    title: React.PropTypes.string.isRequired,
  }

  handleClick(saying) {
    alert(saying);

  }

  render() {
      return (
            <div>
                <img src="http://lorempixel.com/600/400?id={this.props.title}" />
                <h1 className="chris" onClick={this.handleClick.bind(this, this.props.title)}>{this.props.title}</h1>
            </div>
        );
  }
}

export default Title;
