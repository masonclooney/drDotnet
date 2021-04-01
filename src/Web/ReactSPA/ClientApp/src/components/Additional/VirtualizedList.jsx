import React, { Component } from "react";
import classNames from "classnames";
import "./VirtualizedList.css";

const style = {
  listWrapper: (height) => ({
    height,
    position: "relative",
  }),
  item: (index, height) => ({
    height,
    left: 0,
    right: 0,
    top: height * index,
    position: "absolute",
  }),
};

class VirtualizedList extends Component {
  constructor(props) {
    super(props);

    this.listRef = React.createRef();

    this.state = {
      renderIds: new Map(),
      renderIdsList: [],
      viewportHeight: 0,
      scrollTop: 0,
    };
  }

  componentDidMount() {
    window.addEventListener("resize", this.setViewPortHeight, true);

    const { current } = this.listRef;
    if (!current) return;
    current.addEventListener("scroll", this.setScrollPosition, true);

    this.setViewPortHeight();
  }

  componentDidUpdate(prevProps, prevState, snapshot) {
    if (prevProps.source !== this.props.source) {
      this.setViewportHeight();
    }
  }

  componentWillUnmount() {
    window.removeEventListener("resize", this.setViewportHeight);

    const { current } = this.listRef;
    if (!current) return;
    current.removeEventListener("scroll", this.setScrollPosition);
  }

  setViewPortHeight = () => {
    const { source } = this.props;
    const { scrollTop } = this.state;
    const { current } = this.listRef;
    if (!current) return;

    const viewportHeight = parseFloat(
      window.getComputedStyle(current, null).getPropertyValue("height")
    );

    const renderIds = this.getRenderIds(source, viewportHeight, scrollTop);

    this.setState({ viewportHeight, ...renderIds });
  };

  getRenderIds(source, viewportHeight, scrollTop) {
    const renderIds = new Map();
    const renderIdsList = [];

    source.forEach((item, index) => {
      if (this.isVisibleItem(index, viewportHeight, scrollTop)) {
        renderIds.set(index, index);
        renderIdsList.push(index);
      }
    });

    return { renderIds, renderIdsList };
  }

  isVisibleItem = (index, viewportHeight, scrollTop) => {
    const { overScanCount, rowHeight } = this.props;

    const itemTop = index * rowHeight;
    const itemBottom = (index + 1) * rowHeight;

    return (
      itemTop > scrollTop - overScanCount * rowHeight &&
      itemBottom < scrollTop + viewportHeight + overScanCount * rowHeight
    );
  };

  setScrollPosition = (event) => {
    const { source, rowHeight } = this.props;
    const { viewportHeight, scrollTop } = this.state;

    if (Math.abs(event.target.scrollTop - scrollTop) >= rowHeight) {
      const renderIds = this.getRenderIds(
        source,
        viewportHeight,
        event.target.scrollTop
      );

      this.setState({
        scrollTop: event.target.scrollTop,
        ...renderIds,
      });
    }
  };

  render() {
    const { source, renderItem, rowHeight, className } = this.props;
    const { renderIds } = this.state;

    const items = (source || []).map((item, index) => {
      return (
        renderIds.has(index) &&
        renderItem({ index, style: style.item(index, rowHeight) })
      );
    });

    return (
      <div ref={this.listRef} className={classNames("vlist", className)}>
        <div style={style.listWrapper((source || []).length * rowHeight)}>
          {items}
        </div>
      </div>
    );
  }
}

export default VirtualizedList;
