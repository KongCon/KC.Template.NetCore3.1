using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace KC.Template.IRepository
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    public interface IBaseRepository
    {
    }

    public interface IBaseRepository<T> : IBaseRepository
        where T : class
    {
        /// <summary>
        /// 根据ID获取实例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(Object id);

        /// <summary>
        /// 删除实例
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// 批量删除实例
        /// </summary>
        /// <param name="entities"></param>
        void DeleteList(IEnumerable<T> entityList);

        /// <summary>
        /// 更新实例
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// 批量更新实例
        /// </summary>
        /// <param name="entityList">实例列表</param>
        void UpdateList(IEnumerable<T> entityList);

        /// <summary>
        /// 保存实例
        /// </summary>
        void Save(T entity);

        /// <summary>
        /// 批量保存实例
        /// </summary>
        /// <param name="entityList">实例列表</param>
        void SaveList(IEnumerable<T> entityList);

        /// <summary>
        /// 保存或更新实例（待实现）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //T SaveOrUpdate(T entity);

        /// <summary>
        /// 查找实例
        /// </summary>
        /// <param name="filter">过滤方法</param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 统计数量
        /// </summary>
        /// <param name="filter">过滤方法</param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// 根据条件获取数据列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="isAsc">排序字段</param>
        /// <param name="includeList">关联实体</param>
        /// <returns></returns>
        IList<T> GetList(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, bool isAsc = true, IEnumerable<string> includeList = null);

        /// <summary>
        /// 根据条件获取前多少行数据
        /// </summary>
        /// <param name="pageSize">数据数量</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="isAsc">是否升序，默认为true</param>
        /// <param name="includeList">关联实体</param>
        /// <returns></returns>
        IList<T> GetTopList(int pageSize, Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, bool isAsc = true, IEnumerable<Expression<Func<T, object>>> includeList = null);

        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="totalCount">输出总数量</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页数据数量</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="isAsc">是否升序，默认为true</param>
        /// <param name="includeList">关联实体</param>
        /// <returns></returns>
        IList<T> GetPageList(out int totalCount, int page, int pageSize, Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, bool isAsc = true, IEnumerable<string> includeList = null);
    }
}
